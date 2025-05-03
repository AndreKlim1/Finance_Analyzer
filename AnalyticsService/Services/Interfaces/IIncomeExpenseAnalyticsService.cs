using AnalyticsService.Models.DTO.Response.IncomeExpenseReport;
using AnalyticsService.Models.Errors;

namespace AnalyticsService.Services.Interfaces
{
    public interface IIncomeExpenseAnalyticsService
    {
        Task<Result<ComparisonResponse<IncomeExpenseKpiDto>>> GetKpiAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);

        Task<Result<ComparisonResponse<IncomeExpenseTrendResponse>>> GetTrendAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);

        Task<Result<ComparisonResponse<IncomeExpenseBreakdownResponse>>> GetBreakdownAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);

        Task<Result<ComparisonResponse<IEnumerable<TimeTableRowResponse>>>> GetTimeTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);

        Task<Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetSourceTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);

        Task<Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetCategoryTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token);
    }
}
