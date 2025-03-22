namespace CategoryAccountService.Messaging
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, string key, T message);
    }
}
