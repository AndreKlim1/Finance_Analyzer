namespace CategoryAccountService.Messaging.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, string key, T message);
    }
}
