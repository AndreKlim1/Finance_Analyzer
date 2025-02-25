using BudgetingService.Models;

namespace BudgetingService.Repositories.Interfaces
{
    public interface IBudgetRepository : IRepositoryBase<Budget, long>
    {
        public Task<Budget?> GetByEmailAsync(string email, CancellationToken token);
        
    }
}
