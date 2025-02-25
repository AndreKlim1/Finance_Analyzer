using TransactionsService.Repositories.Interfaces;
using TransactionsService.Repositories.Implementations;

namespace TransactionsService.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private TransactionsServiceDbContext _repositoryContext;
        private ITransactionRepository _transactionRepository;

        public RepositoryManager(TransactionsServiceDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                    _transactionRepository = new TransactionRepository(_repositoryContext);
                return _transactionRepository;
            }
        }

        /// <summary>
        /// Метод сохранения общий для всех репозиториев, чтобы сделать сохранения разом
        /// </summary>
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();

    }
}
