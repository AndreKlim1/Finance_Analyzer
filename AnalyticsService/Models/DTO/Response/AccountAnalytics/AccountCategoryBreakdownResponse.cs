namespace AnalyticsService.Models.DTO.Response.AccountAnalytics
{
    public record AccountCategoryBreakdownResponse(
        List<long> Labels,
        List<decimal> Values);
}
