using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CategoryAccountService.Messaging.Kafka
{
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaSettings _settings;

        public KafkaProducer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                Acks = Acks.All,
                BatchSize = _settings.BatchSize,
                LingerMs = _settings.LingerMs,
                CompressionType = _settings.CompressionType
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string topic, string key, T message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = json
            });
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}

