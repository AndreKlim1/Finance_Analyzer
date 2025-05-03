namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class AvgCheckTrendResponse
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Values { get; set; } = new();
    }
}
