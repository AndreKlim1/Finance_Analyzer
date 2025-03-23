using BudgetingService.Models;
using BudgetingService.Models.Errors;
using BudgetingService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetingService.Messaging.Services
{
    public class KafkaBudgetService : IKafkaBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public KafkaBudgetService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<Result<List<Budget>>> GetBudgetsByUserIdAsync(long userId, CancellationToken token)
        {
            var budgets = await _budgetRepository.FindByCondition(l => l.UserId == userId, true).ToListAsync();
            if (budgets is null)
            {
                return Result<List<Budget>>.Failure(BudgetErrors.TransactionNotFound);
            }
            else
            {
                return Result<List<Budget>>.Success(budgets);
            }
        }

        public async Task<Result<Budget>> UpdateBudgetAsync(Budget budget, CancellationToken token)
        {
            budget = await _budgetRepository.UpdateAsync(budget, token);

            return budget is null
                ? Result<Budget>.Failure(BudgetErrors.TransactionNotFound)
                : Result<Budget>.Success(budget);
        }
    }
}
