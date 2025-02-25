using CaregoryAccountService.Repositories.Interfaces;

namespace CaregoryAccountService.Repositories
{
    public interface IRepositoryManager
    {
        ICategoryRepository ProfileRepository { get; }
        IAccountRepository UserRepository { get; }
        Task SaveAsync();
    }
}
