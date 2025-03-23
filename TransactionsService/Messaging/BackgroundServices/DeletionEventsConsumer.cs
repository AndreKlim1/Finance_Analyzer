using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using TransactionsService.Messaging.Events;
using TransactionsService.Services.Interfaces;

namespace TransactionsService.Messaging.BackgroundServices
{
    public class DeletionEventConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<DeletionEventConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DeletionEventConsumerService(IOptions<KafkaSettings> options, ILogger<DeletionEventConsumerService> logger, IServiceProvider serviceProvider)
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
            _consumer.Subscribe(new List<string> { "deletion-events" });
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

                            var deletionEvent = JsonSerializer.Deserialize<DeletionEvent>(consumeResult.Message.Value);
                            if (deletionEvent != null) 
                            {
                                _logger.LogInformation($"Получено событие типа: {deletionEvent.EventType}");

                                switch (deletionEvent.EventType)
                                {
                                    case "account-deleted":
                                        await HandleAccountDeletedAsync(deletionEvent.Id, transactionService, stoppingToken);
                                        break;
                                    case "category-deleted":
                                        await HandleCategoryDeletedAsync(deletionEvent.Id, transactionService, stoppingToken);
                                        break;
                                    default:
                                        _logger.LogWarning($"Неизвестный тип события: {deletionEvent.EventType}");
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

        private async Task HandleAccountDeletedAsync(long id, ITransactionService transactionService, CancellationToken token)
        {
            _logger.LogInformation($"Обработка удаления транзакций для счёта {id}");
            await transactionService.DeleteTransactionsByAccountIdAsync(id, token);
        }

        private async Task HandleCategoryDeletedAsync(long id, ITransactionService transactionService, CancellationToken token)
        {

            _logger.LogInformation($"Обработка удаления транзакций для категории {id}");
            await transactionService.DeleteTransactionsByCategoryIdAsync(id, token);
        }
    }
}
