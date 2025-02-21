using UsersService.Models;

namespace UsersService.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User, long>
    {
        public Task<User?> GetByEmailAsync(string email, CancellationToken token);
        
    }
}
