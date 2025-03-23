using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TransactionsService.Messaging.Events;
using TransactionsService.Models;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.Enums;
using TransactionsService.Services.Interfaces;

namespace TransactionsService.Messaging.BackgroundServices
{
    public class AccountUpdateEventConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<AccountUpdateEventConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AccountUpdateEventConsumer(IOptions<KafkaSettings> options, ILogger<AccountUpdateEventConsumer> logger, IServiceProvider serviceProvider)
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
            _consumer.Subscribe(new List<string> { "account-events" });
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
                            var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();

                            var accEvent = JsonSerializer.Deserialize<AccountUpdateEvent>(consumeResult.Message.Value);
                            if (accEvent != null)
                            {
                                _logger.LogInformation($"Получено событие типа: AccountUpdateEvent");

                                var transaction = new CreateTransactionRequest(accEvent.Value, "Account correction",
                                                                               accEvent.Currency, /*Добавить номер нужной категории*/1,
                                                                               accEvent.Id, accEvent.UserId, $"correction of {accEvent.AccountName} account", "",
                                                                               DateTime.UtcNow, DateTime.UtcNow, TransactionType.CORRECTION.ToString(), "");

                                await transactionService.CreateTransactionFromAccountAsync(transaction, stoppingToken);
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
    }
}
