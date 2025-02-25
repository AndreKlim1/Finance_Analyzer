using TransactionsService.Repositories.Interfaces;

namespace TransactionsService.Repositories
{
    public interface IRepositoryManager
    {
        ITransactionRepository TransactionRepository { get; }
        Task SaveAsync();
    }
}
