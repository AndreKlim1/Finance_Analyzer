using TransactionsService.Models;
using TransactionsService.Models.Enums;

namespace TransactionsService.Repositories.Interfaces
{
    public interface ITransactionRepository : IRepositoryBase<Transaction, long>
    {
        public Task<int?> GetValueByIdAsync(long id, CancellationToken token);
        public Task<Currency?> GetCurrencyByIdAsync(long id, CancellationToken token);
    }
}
