namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class LargestTransactionResponse
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Title { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
    }
}
