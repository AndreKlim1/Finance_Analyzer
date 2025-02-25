using Microsoft.EntityFrameworkCore;
using BudgetingService.Models;
using BudgetingService.Repositories;
using BudgetingService.Repositories.Interfaces;

namespace BudgetingService.Repositories.Implementations
{
    public class BudgetRepository : RepositoryBase<Budget, long>, IBudgetRepository
    {
        private readonly BudgetingServiceDbContext _context;

        public BudgetRepository(BudgetingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Budget?> GetByEmailAsync(string email, CancellationToken token)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.Email == email, token);
        }
    }
}
