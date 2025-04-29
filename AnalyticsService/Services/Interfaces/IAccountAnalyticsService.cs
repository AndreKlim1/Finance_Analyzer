using AnalyticsService.Models.DTO.Response.AccountAnalytics;
using AnalyticsService.Models.Errors;

namespace AnalyticsService.Services.Interfaces
{
    public interface IAccountAnalyticsService
    {
        Task<Result<AccountKpiResponse>> GetAccountKpiAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token);
        Task<Result<AccountBalanceTrendResponse>> GetAccountBalanceTrendAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token);
        Task<Result<AccountCategoryBreakdownResponse>> GetAccountCategoryBreakdownAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string type, CancellationToken token);
    }
}
