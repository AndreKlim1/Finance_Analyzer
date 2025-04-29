namespace AnalyticsService.Models.DTO.Response.AccountAnalytics
{
    public record AccountKpiResponse(
        decimal StartBalance,
        decimal Inflow,
        decimal Outflow,
        decimal NetChange,
        decimal EndBalance);
}
