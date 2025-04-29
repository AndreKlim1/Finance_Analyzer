namespace AnalyticsService.Messaging.DTO
{
    public class TransactionResponse
    {
        public long Id { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public string Currency { get; set; }
        public long CategoryId { get; set; }
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string TransactionType { get; set; }
        public string? Merchan { get; set; }

    }
    
}
