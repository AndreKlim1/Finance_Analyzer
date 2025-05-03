using BudgetingService.Messaging.DTO;
using BudgetingService.Messaging.Http;
using BudgetingService.Messaging.Services;
using BudgetingService.Models.Enums;
using BudgetingService.Services.Implementations;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BudgetingService.Messaging.BackgroundServices
{

    public class TransactionUpdateEventsConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TransactionUpdateEventsConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TransactionUpdateEventsConsumer(IOptions<KafkaSettings> options, ILogger<TransactionUpdateEventsConsumer> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var kafkaSettings = options.Value;
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                GroupId = kafkaSettings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _logger = logger;
            _consumer.Subscribe(new List<string> { "transaction-update-events" });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var budgetService = scope.ServiceProvider.GetRequiredService<IKafkaBudgetService>();
                            var conversionService = scope.ServiceProvider.GetRequiredService<ICurrencyConversionClient>();

                            var transactionEvent = JsonSerializer.Deserialize<TransactionUpdateEvent>(consumeResult.Message.Value);
                            if (transactionEvent != null)
                            {
                                _logger.LogInformation($"Получено событие типа: update-event");
                                await HandleTransactionUpdatedAsync(transactionEvent, budgetService, conversionService, stoppingToken);
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Ошибка потребления: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка обработки сообщения");
                    }
                }
            }, stoppingToken);
        }



        private async Task HandleTransactionUpdatedAsync(TransactionUpdateEvent data, IKafkaBudgetService budgetService, ICurrencyConversionClient conversionClient, CancellationToken token)
        {

            _logger.LogInformation($"Обработка обновления транзакции {data.CurrTransaction.TransactionId} для бюджетов");
            var budgets = await budgetService.GetBudgetsByUserIdAsync(data.CurrTransaction.UserId, token);
            if (budgets.IsSuccess)
            {
                foreach (var budget in budgets.Value)
                {
                    var containsPrev = false;
                    var containsCurr = false;
                    if (((((budget.BudgetType == BudgetType.SAVINGS && data.PrevTransaction.TransactionType == "INCOME") ||
                           (budget.BudgetType == BudgetType.EXPENSES && data.PrevTransaction.TransactionType == "EXPENSE")) &&
                            budget.CategoryIds is null) || budget.CategoryIds.Contains(data.PrevTransaction.CategoryId)) &&
                           (budget.AccountIds is null || budget.AccountIds.Contains(data.PrevTransaction.AccountId)) && 
                            data.PrevTransaction.TransactionDate <= budget.PeriodEnd && data.PrevTransaction.TransactionDate >= budget.PeriodStart)
                    {
                        containsPrev = true;
                    }

                    if (((((budget.BudgetType == BudgetType.SAVINGS && data.CurrTransaction.TransactionType == "INCOME") ||
                           (budget.BudgetType == BudgetType.EXPENSES && data.CurrTransaction.TransactionType == "EXPENSE")) &&
                            budget.CategoryIds is null) || budget.CategoryIds.Contains(data.CurrTransaction.CategoryId)) &&
                           (budget.AccountIds is null || budget.AccountIds.Contains(data.CurrTransaction.AccountId)) &&
                            data.CurrTransaction.TransactionDate <= budget.PeriodEnd && data.CurrTransaction.TransactionDate >= budget.PeriodStart)
                    {
                        containsCurr = true;
                    }

                    var updateValue = 0m;
                    if(containsPrev && containsCurr)
                    {
                        if (budget.Currency.ToString() != data.CurrTransaction.Currency)
                        {
                            updateValue = Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.CurrTransaction.Currency, data.CurrTransaction.Value));
                        }
                        else
                        {
                            updateValue = Math.Abs(data.CurrTransaction.Value);
                        }

                        if (budget.Currency.ToString() != data.PrevTransaction.Currency)
                        {
                            updateValue -= Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.PrevTransaction.Currency, data.PrevTransaction.Value));
                        }
                        else
                        {
                            updateValue -= Math.Abs(data.PrevTransaction.Value);
                        }

                        budget.CurrValue += updateValue;
                        var percent = budget.CurrValue / budget.PlannedAmount * 100;
                        if (percent > budget.WarningThreshold && !budget.WarningShowed)
                        {
                            budget.WarningShowed = true;
                            //отправка запроса в NotificationService для вывода уведомления пользователю
                        }
                        else if (budget.WarningShowed && percent < budget.WarningThreshold)
                        {
                            budget.WarningShowed = false;
                        }
                        await budgetService.UpdateBudgetAsync(budget, token);
                    }
                    else if(containsPrev && !containsCurr)
                    {
                        
                        if (budget.Currency.ToString() != data.PrevTransaction.Currency)
                        {
                            updateValue -= Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.PrevTransaction.Currency, data.PrevTransaction.Value));
                        }
                        else
                        {
                            updateValue -= Math.Abs(data.PrevTransaction.Value);
                        }

                        budget.CurrValue += updateValue;
                        var percent = budget.CurrValue / budget.PlannedAmount * 100;

                        if (budget.WarningShowed && percent < budget.WarningThreshold)
                        {
                            budget.WarningShowed = false;
                        }
                        await budgetService.UpdateBudgetAsync(budget, token);
                    }
                    else if(!containsPrev && containsCurr)
                    {
                        if (budget.Currency.ToString() != data.CurrTransaction.Currency)
                        {
                            updateValue = Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.CurrTransaction.Currency, data.CurrTransaction.Value));
                        }
                        else
                        {
                            updateValue = Math.Abs(data.CurrTransaction.Value);
                        }

                        budget.CurrValue += updateValue;
                        var percent = budget.CurrValue / budget.PlannedAmount * 100;
                        if (percent > budget.WarningThreshold && !budget.WarningShowed)
                        {
                            budget.WarningShowed = true;
                            //отправка запроса в NotificationService для вывода уведомления пользователю
                        }
                        
                        await budgetService.UpdateBudgetAsync(budget, token);
                    }
                }
            }
        }


    }

}
