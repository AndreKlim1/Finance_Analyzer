namespace AnalyticsService.Models.DTO.Response.IncomeExpenseReport
{
    public class IncomeExpenseTrendResponse
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> IncomeSeries { get; set; } = new();
        public List<decimal> ExpenseSeries { get; set; } = new();
    }
}
