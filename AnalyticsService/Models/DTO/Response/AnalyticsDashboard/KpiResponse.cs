namespace AnalyticsService.Models.DTO.Response.AnalyticsDashboard
{
    public record KpiResponse(
        decimal Income,
        decimal Expense,
        decimal NetFlow);
}
