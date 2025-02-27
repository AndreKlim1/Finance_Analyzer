using Microsoft.EntityFrameworkCore;
using CaregoryAccountService.Models;
using CaregoryAccountService.Repositories.Interfaces;

namespace CaregoryAccountService.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account, long>, IAccountRepository
    {
        private readonly CategoryAccountServiceDbContext _context;

        public AccountRepository(CategoryAccountServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
