namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class TimeOfDayPatternResponse
    {
        public string PeriodName { get; set; } = default!;
        public decimal TotalAmount { get; set; }
    }
}
