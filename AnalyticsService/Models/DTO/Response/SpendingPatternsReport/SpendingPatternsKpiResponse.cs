namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class SpendingPatternsKpiResponse
    {
        public decimal TotalSpending { get; set; }
        public decimal AvgDailySpending { get; set; }
        public int TransactionCount { get; set; }
        public decimal AvgCheck { get; set; }
        public string? BusiestDay { get; set; }
    }
}
