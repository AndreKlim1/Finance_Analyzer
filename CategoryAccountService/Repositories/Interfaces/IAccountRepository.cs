using CaregoryAccountService.Models;

namespace CaregoryAccountService.Repositories.Interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account, long>
    {
        //public Task<int> GetBalanceAsync(long id, CancellationToken token);
        //public Task<List<Account>?> GetOverviewByUserIdAsync(long userId, CancellationToken token);
        
    }
}
