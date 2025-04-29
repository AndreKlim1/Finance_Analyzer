namespace AnalyticsService.Models.DTO.Response.AnalyticsDashboard
{
    public record IncomeExpenseTrendResponse(
        List<string> Labels,
        List<decimal> IncomeSeries,
        List<decimal> ExpenseSeries);
    
}
