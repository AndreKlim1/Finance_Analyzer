using UsersService.Repositories.Interfaces;

namespace UsersService.Repositories
{
    public interface IRepositoryManager
    {
        IProfileRepository ProfileRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
