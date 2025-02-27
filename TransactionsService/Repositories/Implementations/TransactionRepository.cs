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

    }
}
