namespace AnalyticsService.Models.DTO.Response.BudgetAnalytics
{
    public record BudgetTrendResponse(
        List<string> Labels,
        List<decimal> Series);
}
