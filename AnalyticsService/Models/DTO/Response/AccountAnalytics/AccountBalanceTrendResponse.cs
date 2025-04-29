namespace AnalyticsService.Models.DTO.Response.AccountAnalytics
{
    public record AccountBalanceTrendResponse(
        List<string> Labels,
        List<decimal> Values);
}
