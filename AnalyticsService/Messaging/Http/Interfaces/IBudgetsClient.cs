using AnalyticsService.Messaging.DTO;

namespace AnalyticsService.Messaging.Http.Interfaces
{
    public interface IBudgetsClient
    {
        Task<BudgetResponse> GetBudgetByIdAsync(long budgetId, CancellationToken cancellationToken);
    }
}
