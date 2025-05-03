using AnalyticsService.Models.DTO.Response.SpendingPatternsReport;
using AnalyticsService.Models.Errors;

namespace AnalyticsService.Services.Interfaces
{
    public interface ISpendingPatternsReportService
    {
        Task<Result<AvgCheckTrendResponse>> GetAvgCheckTrendAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
        Task<Result<List<DayOfWeekPatternResponse>>> GetDayOfWeekPatternsAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
        Task<Result<List<LargestTransactionResponse>>> GetLargestTransactionsAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
        Task<Result<SpendingPatternsKpiResponse>> GetSpendingPatternsKpiAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
        Task<Result<List<TimeOfDayPatternResponse>>> GetTimeOfDayPatternsAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
        Task<Result<ValueDistributionResponse>> GetValueDistributionAsync(DateTime startDate, DateTime endDate, string targetCurrency, string? accountIds, string? categoryIds, CancellationToken token);
    }
}
