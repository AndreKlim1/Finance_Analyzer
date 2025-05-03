namespace AnalyticsService.Models.DTO.Response.IncomeExpenseReport
{
    public class BreakdownDto
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Values { get; set; } = new();
    }
}
