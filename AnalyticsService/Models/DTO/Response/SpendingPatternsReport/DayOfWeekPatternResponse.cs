namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class DayOfWeekPatternResponse
    {
        public int DayIndex { get; set; }
        public string DayName { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public decimal AvgAmount { get; set; }
        public int Count { get; set; }
    }
}
