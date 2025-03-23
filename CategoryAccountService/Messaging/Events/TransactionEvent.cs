namespace CategoryAccountService.Messaging.Events
{
    public class TransactionEvent
    {
        public string EventType { get; set; } 
        public TransactionData Data { get; set; }
    }

    public class TransactionData
    {
        public long TransactionId { get; set; }
        public long UserId { get; set; }
        public int Value { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public long AccountId { get; set; }
        public long CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
