namespace AnalyticsService.Models.DTO.Response.IncomeExpenseReport
{
    public class IncomeExpenseBreakdownResponse
    {
        public BreakdownDto Income { get; set; } = default!;
        public BreakdownDto Expense { get; set; } = default!;
    }
}
