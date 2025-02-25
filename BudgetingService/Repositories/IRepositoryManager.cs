using BudgetingService.Repositories.Interfaces;

namespace BudgetingService.Repositories
{
    public interface IRepositoryManager
    {
        IBudgetRepository TransactionRepository { get; }
        Task SaveAsync();
    }
}
