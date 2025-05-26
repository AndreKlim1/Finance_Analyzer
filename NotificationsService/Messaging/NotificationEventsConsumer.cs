using Confluent.Kafka;
using Microsoft.Extensions.Options;
using NotificationsService.Services;
using System.Text.Json;

namespace NotificationsService.Messaging
{
    public class NotificationEventsConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<NotificationEventsConsumer> _logger;
        private readonly IServiceProvider _provider;

        public NotificationEventsConsumer(
            IOptions<KafkaSettings> options,
            ILogger<NotificationEventsConsumer> log,
            IServiceProvider provider)
        {
            _logger = log;
            _provider = provider;
            var kafkaSettings = options.Value;
            var cfg = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                GroupId = kafkaSettings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
            _consumer = new ConsumerBuilder<string, string>(cfg).Build();
            _consumer.Subscribe("notifications-events");
        }

        protected override Task ExecuteAsync(CancellationToken token)
            => Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var msg = _consumer.Consume(token);
                        var evt = JsonSerializer.Deserialize<NotificationEvent>(msg.Message.Value);
                        if (evt != null)
                        {
                            using var scope = _provider.CreateScope();
                            var svc = scope.ServiceProvider.GetRequiredService<INotificationService>();
                            _logger.LogInformation($"Notify {evt.UserId}: {evt.Message}");
                            await svc.SendToUserAsync(evt.UserId, evt.Message);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError(e, "Kafka consume error");
                    }
                }
            }, token);
    }
}
