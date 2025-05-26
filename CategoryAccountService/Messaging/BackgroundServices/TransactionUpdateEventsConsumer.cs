using CaregoryAccountService.Services.Interfaces;
using CategoryAccountService.Messaging.DTO;
using CategoryAccountService.Messaging.Http;
using CategoryAccountService.Messaging.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CategoryAccountService.Messaging.BackgroundServices
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
                            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                            var conversionService = scope.ServiceProvider.GetRequiredService<ICurrencyConversionClient>();
                            var transactionEvent = JsonSerializer.Deserialize<TransactionUpdateEvent>(consumeResult.Message.Value);
                            if (transactionEvent != null)
                            {
                                _logger.LogInformation($"Получено событие типа: transactin-update");
                                await HandleTransactionUpdatedAsync(transactionEvent, accountService, conversionService, stoppingToken);

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

        private async Task HandleTransactionUpdatedAsync(TransactionUpdateEvent data, IAccountService accountService, ICurrencyConversionClient conversionClient, CancellationToken token)
        {

            _logger.LogInformation($"Обработка обновления транзакции {data.CurrTransaction.TransactionId} для счета {data.CurrTransaction.AccountId}");
            var prevAccountCurrency = await accountService.GetCurrencyByIdAsync(data.PrevTransaction.AccountId, token);
            var currAccountCurrency = await accountService.GetCurrencyByIdAsync(data.CurrTransaction.AccountId, token);
            var updateValue = 0m;
            
            if(data.CurrTransaction.AccountId != data.PrevTransaction.AccountId) 
            {
                if(prevAccountCurrency.IsFailure || currAccountCurrency.IsFailure)
                {
                    throw new Exception("Ошибка получения валюты счёта");
                }

                updateValue = await conversionClient.ConvertTransactionValueAsync(prevAccountCurrency.Value, data.PrevTransaction.Currency, data.PrevTransaction.Value);
                await accountService.UpdateBalanceAsync(data.PrevTransaction.AccountId, -updateValue, -1, token);

                updateValue = await conversionClient.ConvertTransactionValueAsync(currAccountCurrency.Value, data.CurrTransaction.Currency, data.CurrTransaction.Value);
                await accountService.UpdateBalanceAsync(data.CurrTransaction.AccountId, -updateValue, 1, token);

            }
            else
            {
                var prevConvertedValue = await conversionClient.ConvertTransactionValueAsync(currAccountCurrency.Value, data.PrevTransaction.Currency, data.PrevTransaction.Value);
                var currConvertedValue = await conversionClient.ConvertTransactionValueAsync(currAccountCurrency.Value, data.CurrTransaction.Currency, data.CurrTransaction.Value);
                updateValue = currConvertedValue - prevConvertedValue;
                await accountService.UpdateBalanceAsync(data.CurrTransaction.AccountId, -updateValue, 0, token);
            }
        }
    }
}
