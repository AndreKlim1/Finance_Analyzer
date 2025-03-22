using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CategoryAccountService.Messaging
{
    public class KafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, string> _consumer;

        public KafkaConsumer(IOptions<KafkaSettings> options)
        {
            var settings = options.Value;
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = settings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<TKey, string>(config).Build();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            _consumer.Subscribe(topics);
        }

        public void Consume(CancellationToken cancellationToken, Action<ConsumeResult<TKey, string>> handleMessage)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    handleMessage(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Ошибка потребления: {ex.Error.Reason}");
                }
            }
        }
    }
}
