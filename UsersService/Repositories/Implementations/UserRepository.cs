using Microsoft.EntityFrameworkCore;
using UsersService.Models;
using UsersService.Repositories.Interfaces;

namespace UsersService.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User, long>, IUserRepository
    {
        private readonly UsersServiceDbContext _context;

        public UserRepository(UsersServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, token);
        }
    }
}
