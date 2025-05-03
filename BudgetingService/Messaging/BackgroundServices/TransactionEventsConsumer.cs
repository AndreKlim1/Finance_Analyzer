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
    public class TransactionEventConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TransactionEventConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TransactionEventConsumerService(IOptions<KafkaSettings> options, ILogger<TransactionEventConsumerService> logger, IServiceProvider serviceProvider)
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
            _consumer.Subscribe(new List<string> { "transaction-events" });
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
                            var transactionEvent = JsonSerializer.Deserialize<TransactionEvent>(consumeResult.Message.Value);
                            if (transactionEvent != null)
                            {
                                _logger.LogInformation($"Получено событие типа: {transactionEvent.EventType} для транзакции {transactionEvent.Data.TransactionId}");

                                switch (transactionEvent.EventType)
                                {
                                    case "transaction-created":
                                        await HandleTransactionCreatedAsync(transactionEvent.Data, budgetService, conversionService, stoppingToken);
                                        break;
                                    case "transaction-deleted":
                                        await HandleTransactionDeletedAsync(transactionEvent.Data, budgetService, conversionService, stoppingToken);
                                        break;
                                    default:
                                        _logger.LogWarning($"Неизвестный тип события: {transactionEvent.EventType}");
                                        break;
                                }
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

        private async Task HandleTransactionCreatedAsync(TransactionData data, IKafkaBudgetService budgetService, ICurrencyConversionClient conversionClient, CancellationToken token)
        {
            _logger.LogInformation($"Обработка создания транзакции {data.TransactionId} для бюджетов");
            var budgets = await budgetService.GetBudgetsByUserIdAsync(data.UserId, token);
            if (budgets.IsSuccess)
            {
                foreach (var budget in budgets.Value)
                {
                    if (((((budget.BudgetType == BudgetType.SAVINGS && data.TransactionType == "INCOME") ||
                           (budget.BudgetType == BudgetType.EXPENSES && data.TransactionType == "EXPENSE")) &&
                            budget.CategoryIds is null) || budget.CategoryIds.Contains(data.CategoryId)) &&
                           (budget.AccountIds is null || budget.AccountIds.Contains(data.AccountId)) &&
                            data.TransactionDate <= budget.PeriodEnd && data.TransactionDate >= budget.PeriodStart)
                    {
                        var updateValue = 0m;
                        if (budget.Currency.ToString() != data.Currency)
                        {
                            updateValue = Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.Currency, data.Value));
                        }
                        else
                        {
                            updateValue = Math.Abs(data.Value);
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

        private async Task HandleTransactionDeletedAsync(TransactionData data, IKafkaBudgetService budgetService, ICurrencyConversionClient conversionClient, CancellationToken token)
        {
            _logger.LogInformation($"Обработка удаления транзакции {data.TransactionId}");
            var budgets = await budgetService.GetBudgetsByUserIdAsync(data.UserId, token);
            if (budgets.IsSuccess)
            {
                foreach (var budget in budgets.Value)
                {
                    if (((((budget.BudgetType == BudgetType.SAVINGS && data.TransactionType == "INCOME") ||
                           (budget.BudgetType == BudgetType.EXPENSES && data.TransactionType == "EXPENSE")) &&
                            budget.CategoryIds is null) || budget.CategoryIds.Contains(data.CategoryId)) &&
                           (budget.AccountIds is null || budget.AccountIds.Contains(data.AccountId)) &&
                            data.TransactionDate <= budget.PeriodEnd && data.TransactionDate >= budget.PeriodStart)
                    {
                        var updateValue = 0m;
                        if (budget.Currency.ToString() != data.Currency)
                        {
                            updateValue = Math.Abs(await conversionClient.ConvertTransactionValueAsync(budget.Currency.ToString(), data.Currency, data.Value));
                        }
                        else
                        {
                            updateValue = Math.Abs(data.Value);
                        }

                        budget.CurrValue -= updateValue;
                        var percent = budget.CurrValue / budget.PlannedAmount * 100;
                        if (budget.WarningShowed && percent < budget.WarningThreshold)
                        {
                            budget.WarningShowed = false;
                            //отправка запроса в NotificationService для вывода уведомления пользователю
                        }
                        await budgetService.UpdateBudgetAsync(budget, token);
                    }
                }
            }
        }
    }
}
