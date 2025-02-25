using CaregoryAccountService.Repositories.Implementations;
using CaregoryAccountService.Repositories.Interfaces;

namespace CaregoryAccountService.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private CategoryAccountServiceDbContext _repositoryContext;
        private ICategoryRepository _profileRepository;
        private IAccountRepository _userRepository;

        public RepositoryManager(CategoryAccountServiceDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IAccountRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new AccountRepository(_repositoryContext);
                return _userRepository;
            }
        }

        public ICategoryRepository ProfileRepository
        {
            get
            {
                if (_profileRepository == null)
                    _profileRepository = new CategoryRepository(_repositoryContext);
                return _profileRepository;
            }
        }

        /// <summary>
        /// Метод сохранения общий для всех репозиториев, чтобы сделать сохранения разом
        /// </summary>
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();

    }
}
