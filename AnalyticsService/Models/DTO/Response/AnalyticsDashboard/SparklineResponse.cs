namespace AnalyticsService.Models.DTO.Response.AnalyticsDashboard
{
    public record SparklineResponse(
        List<string> Labels,
        List<decimal> Values);
}
