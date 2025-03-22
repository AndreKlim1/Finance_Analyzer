using Confluent.Kafka;

namespace BudgetingService.Messaging
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string ConsumerGroupId { get; set; }
        public int BatchSize { get; set; } = 16384; // 16 КБ
        public int LingerMs { get; set; } = 5; // 5 мс
        public CompressionType CompressionType { get; set; } = CompressionType.Snappy;
    }
}
