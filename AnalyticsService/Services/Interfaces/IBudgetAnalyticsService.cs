using AnalyticsService.Models.DTO.Response.BudgetAnalytics;
using AnalyticsService.Models.Errors;

namespace AnalyticsService.Services.Interfaces
{
    public interface IBudgetAnalyticsService
    {
        Task<Result<BudgetTrendResponse>> GetBudgetTrendAsync(long userId, long budgetId, DateTime startDate, DateTime endDate, CancellationToken token);
        Task<Result<BudgetCategoryBreakdownResponse>> GetBudgetCategoryBreakdownAsync(long userId, long budgetId, DateTime startDate, DateTime endDate, CancellationToken token);

    }
}
