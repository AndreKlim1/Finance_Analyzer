using TransactionsService.Models;

namespace TransactionsService.Repositories.Interfaces
{
    public interface ITransactionRepository : IRepositoryBase<Transaction, long>
    {
        public Task<Transaction?> GetByEmailAsync(string email, CancellationToken token);
        
    }
}
