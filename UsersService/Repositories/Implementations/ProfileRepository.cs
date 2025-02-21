using UsersService.Models;
using UsersService.Repositories.Interfaces;

namespace UsersService.Repositories.Implementations
{
    public class ProfileRepository : RepositoryBase<Profile, long>, IProfileRepository
    {
        private readonly UsersServiceDbContext _context;

        public ProfileRepository(UsersServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
