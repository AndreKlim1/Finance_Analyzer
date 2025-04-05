namespace IntegrationService.DTO
{
    public class CSVTransactionDTO
    {
        public string Title { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public string CategoryName { get; set; }
        public string AccountName { get; set; }
        public string? Description { get; set; }
        public string? Merchant { get; set; }
    }
}
