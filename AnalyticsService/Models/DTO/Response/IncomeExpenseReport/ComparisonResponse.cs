namespace AnalyticsService.Models.DTO.Response.IncomeExpenseReport
{
    public class ComparisonResponse<T>
    {
        public T PrimaryData { get; set; } = default!;
        public T ComparisonData { get; set; } = default!;
    }
}
