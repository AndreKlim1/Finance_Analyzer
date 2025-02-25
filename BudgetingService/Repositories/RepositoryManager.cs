using BudgetingService.Repositories.Interfaces;
using BudgetingService.Repositories.Implementations;

namespace BudgetingService.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private BudgetingServiceDbContext _repositoryContext;
        private IBudgetRepository _transactionRepository;

        public RepositoryManager(BudgetingServiceDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IBudgetRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                    _transactionRepository = new BudgetRepository(_repositoryContext);
                return _transactionRepository;
            }
        }

        /// <summary>
        /// Метод сохранения общий для всех репозиториев, чтобы сделать сохранения разом
        /// </summary>
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();

    }
}
