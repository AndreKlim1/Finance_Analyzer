using AnalyticsService.Models.DTO.Response.AnalyticsDashboard;
using AnalyticsService.Models.Errors;

namespace AnalyticsService.Services.Interfaces
{
    public interface IAnalyticsDashboardService
    {
        Task<Result<KpiResponse>> GetKpiAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token);
        Task<Result<IncomeExpenseTrendResponse>> GetIncomeExpenseTrendAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string granularity, CancellationToken token);
        Task<Result<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string type, CancellationToken token);
        Task<Result<SparklineResponse>> GetAccountSparklineAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token);
    }
}
