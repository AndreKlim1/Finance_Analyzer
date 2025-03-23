using TransactionsService.Models;

namespace TransactionsService.Messaging.Events
{
    public class TransactionEvent
    {
        public TransactionEvent(string eventType, TransactionData data)
        {
            EventType = eventType;
            Data = data;
        }
        public string EventType { get; set; }
        public TransactionData Data { get; set; }
    }

    public class TransactionData
    {
        public TransactionData(Transaction transaction) 
        {
            TransactionId = transaction.Id;
            UserId = transaction.UserId;
            Value = transaction.Value;
            Currency = transaction.Currency.ToString();
            TransactionType = transaction.TransactionType.ToString();
            AccountId = transaction.AccountId;
            CategoryId = transaction.CategoryId;
            TransactionDate = transaction.TransactionDate;
        }
        public long TransactionId { get; set; }
        public long UserId { get; set; }
        public int Value { get; set; }
        public string Currency { get; set; }
        public string TransactionType { get; set; }
        public long AccountId { get; set; }
        public long CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
