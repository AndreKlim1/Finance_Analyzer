namespace AnalyticsService.Models.DTO.Response.AnalyticsDashboard
{
    public record CategoryBreakdownResponse(
        List<long> Labels,
        List<decimal> Values);
}
