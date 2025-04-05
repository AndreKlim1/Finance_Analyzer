using CaregoryAccountService.Models;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Repositories.Interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account, long>
    {
        public Task<decimal?> GetBalanceByIdAsync(long id, CancellationToken token);
        public Task<Currency?> GetCurrencyByIdAsync(long id, CancellationToken token);
        //public Task<List<Account>?> GetOverviewByUserIdAsync(long userId, CancellationToken token);

    }
}
