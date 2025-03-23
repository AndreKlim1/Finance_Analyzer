using FluentValidation;
using BudgetingService.Services.Interfaces;
using BudgetingService.Repositories.Interfaces;
using BudgetingService.Models.DTO.Requests;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.Errors;
using BudgetingService.Services.Mappings;
using BudgetingService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BudgetingService.Services.Implementations
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<Result<BudgetResponse>> GetBudgetByIdAsync(long id, CancellationToken token)
        {
            var budget = await _budgetRepository.GetByIdAsync(id, token);

            return budget is null
                ? Result<BudgetResponse>.Failure(BudgetErrors.TransactionNotFound)
                : Result<BudgetResponse>.Success(budget.ToBudgetResponse());
        }

        public async Task<Result<BudgetResponse>> CreateBudgetAsync(CreateBudgetRequest createBudgetRequest,
            CancellationToken token)
        {

            var budget = createBudgetRequest.ToBudget();

            await _budgetRepository.AddAsync(budget, token);

            return Result<BudgetResponse>.Success(budget.ToBudgetResponse());
        }

        public async Task<Result<BudgetResponse>> UpdateBudgetAsync(UpdateBudgetRequest updateBudgetRequest,
            CancellationToken token)
        {

            var budget = updateBudgetRequest.ToBudget();
            budget = await _budgetRepository.UpdateAsync(budget, token);

            return budget is null
                ? Result<BudgetResponse>.Failure(BudgetErrors.TransactionNotFound)
                : Result<BudgetResponse>.Success(budget.ToBudgetResponse());
        }

        public async Task<bool> DeleteBudgetAsync(long id, CancellationToken token)
        {
            await _budgetRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<List<BudgetResponse>>> GetBudgetsAsync(CancellationToken token)
        {
            var budgets = await _budgetRepository.FindAll(true).ToListAsync();
            if (budgets is null)
            {
                return Result<List<BudgetResponse>>.Failure(BudgetErrors.TransactionNotFound);
            }
            else
            {
                var responses = new List<BudgetResponse>();
                foreach (var budget in budgets)
                {
                    responses.Add(budget.ToBudgetResponse());
                }
                return Result<List<BudgetResponse>>.Success(responses);
            }
        }

        public async Task<Result<List<BudgetResponse>>> GetBudgetsByUserIdAsync(long userId, CancellationToken token)
        {
            var budgets = await _budgetRepository.FindByCondition(l => l.UserId==userId, true).ToListAsync();
            if (budgets is null)
            {
                return Result<List<BudgetResponse>>.Failure(BudgetErrors.TransactionNotFound);
            }
            else
            {
                var responses = new List<BudgetResponse>();
                foreach (var budget in budgets)
                {
                    responses.Add(budget.ToBudgetResponse());
                }
                return Result<List<BudgetResponse>>.Success(responses);
            }
        }
    }
}
