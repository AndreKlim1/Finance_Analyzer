using UsersService.Repositories.Implementations;
using UsersService.Repositories.Interfaces;

namespace UsersService.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private UsersServiceDbContext _repositoryContext;
        private IProfileRepository _profileRepository;
        private IUserRepository _userRepository;

        public RepositoryManager(UsersServiceDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext);
                return _userRepository;
            }
        }

        public IProfileRepository ProfileRepository
        {
            get
            {
                if (_profileRepository == null)
                    _profileRepository = new ProfileRepository(_repositoryContext);
                return _profileRepository;
            }
        }

        /// <summary>
        /// Метод сохранения общий для всех репозиториев, чтобы сделать сохранения разом
        /// </summary>
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();

    }
}
