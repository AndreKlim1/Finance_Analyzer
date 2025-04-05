using Microsoft.EntityFrameworkCore;
using TransactionsService.Models;
using TransactionsService.Models.Enums;
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

        public async Task<decimal?> GetValueByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id, token);
            if (transaction != null) 
            {
                return transaction.Value;
            }
            else
            {
                return null;    
            }
        }

        public async Task<Currency?> GetCurrencyByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id, token);
            if (transaction != null)
            {
                return transaction.Currency;
            }
            else
            {
                return null;
            }
        }

        public async Task<TransactionType?> GetTypeByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id, token);
            if (transaction != null)
            {
                return transaction.TransactionType;
            }
            else
            {
                return null;
            }
        }
    }
}
