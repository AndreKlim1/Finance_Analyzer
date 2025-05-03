namespace AnalyticsService.Models.DTO.Response.IncomeExpenseReport
{
    public class TimeTableRowResponse
    {
        public string Period { get; set; } = default!;
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
