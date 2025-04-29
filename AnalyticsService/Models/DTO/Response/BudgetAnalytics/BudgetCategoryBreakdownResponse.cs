namespace AnalyticsService.Models.DTO.Response.BudgetAnalytics
{
    public record BudgetCategoryBreakdownResponse(
        List<long> Labels,
        List<decimal> Values);
}
