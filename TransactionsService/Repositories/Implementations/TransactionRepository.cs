using Microsoft.EntityFrameworkCore;
using TransactionsService.Models;
using TransactionsService.Repositories;
using TransactionsService.Repositories.Interfaces;

namespace TransactionsService.Repositories.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction, long>, ITransactionRepository
    {
        private readonly TransactionsServiceDbContext _context;

        public TransactionRepository(TransactionsServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByEmailAsync(string email, CancellationToken token)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.Email == email, token);
        }
    }
}
