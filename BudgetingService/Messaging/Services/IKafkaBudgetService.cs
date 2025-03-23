using BudgetingService.Models;
using BudgetingService.Models.Errors;

namespace BudgetingService.Messaging.Services
{
    public interface IKafkaBudgetService
    {
        Task<Result<List<Budget>>> GetBudgetsByUserIdAsync(long userId, CancellationToken token);
        Task<Result<Budget>> UpdateBudgetAsync(Budget budget, CancellationToken token);
    }
}
