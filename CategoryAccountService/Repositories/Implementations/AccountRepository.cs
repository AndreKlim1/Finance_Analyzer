using Microsoft.EntityFrameworkCore;
using CaregoryAccountService.Models;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account, long>, IAccountRepository
    {
        private readonly CategoryAccountServiceDbContext _context;

        public AccountRepository(CategoryAccountServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal?> GetBalanceByIdAsync(long id, CancellationToken token)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, token);
            if (account != null)
            {
                return account.Balance;
            }
            else
            {
                return null;
            }
        }

        public async Task<Currency?> GetCurrencyByIdAsync(long id, CancellationToken token)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, token);
            if (account != null)
            {
                return account.Currency;
            }
            else
            {
                return null;
            }
        }
    }
}
