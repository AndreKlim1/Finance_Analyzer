using CaregoryAccountService.Services.Interfaces;
using CategoryAccountService.Messaging.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CategoryAccountService.Messaging.BackgroundServices
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
                            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                            var transactionEvent = JsonSerializer.Deserialize<TransactionEvent>(consumeResult.Message.Value);
                            if (transactionEvent != null)
                            {
                                _logger.LogInformation($"Получено событие типа: {transactionEvent.EventType} для транзакции {transactionEvent.Data.TransactionId}");

                                switch (transactionEvent.EventType)
                                {
                                    case "transaction-created":
                                        await HandleTransactionCreatedAsync(transactionEvent.Data, accountService, stoppingToken);
                                        break;
                                    case "transaction-deleted":
                                        await HandleTransactionDeletedAsync(transactionEvent.Data, accountService, stoppingToken);
                                        break;
                                    case "transaction-updated":
                                        await HandleTransactionUpdatedAsync(transactionEvent.Data, accountService, stoppingToken);
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

        private async Task HandleTransactionCreatedAsync(TransactionData data, IAccountService accountService, CancellationToken token)
        {
            _logger.LogInformation($"Обработка создания транзакции {data.TransactionId} для счета {data.AccountId}");
            //проверка соответствия валют
            await accountService.UpdateBalanceAsync(data.AccountId, data.Value, 1, token);
        }

        private async Task HandleTransactionUpdatedAsync(TransactionData data, IAccountService accountService, CancellationToken token)
        {

            _logger.LogInformation($"Обработка обновления транзакции {data.TransactionId} для счета {data.AccountId}");
            //проверка соответствия валют

            await accountService.UpdateBalanceAsync(data.AccountId, data.Value, 0, token);

        }

        private async Task HandleTransactionDeletedAsync(TransactionData data, IAccountService accountService, CancellationToken token)
        {
            _logger.LogInformation($"Обработка удаления транзакции {data.TransactionId}");
            //проверка соответствия валют
            await accountService.UpdateBalanceAsync(data.AccountId, data.Value, -1, token);
        }
    }
}
