using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;

namespace CaregoryAccountService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountResponse>> GetAccountByIdAsync(long id, CancellationToken token);
        Task<Result<AccountResponse>> CreateAccountAsync(CreateAccountRequest createUserRequest, CancellationToken token);
        Task<Result<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest updateUserRequest, CancellationToken token);
        Task<Result<List<AccountResponse>>> GetAccountsAsync(CancellationToken token);
        Task<bool> DeleteAccountAsync(long id, CancellationToken token);
        Task<Result<AccountResponse>> UpdateBalanceAsync(long id, decimal value, int trCountChange, CancellationToken token);
        Task<Result<string>> GetCurrencyByIdAsync(long id, CancellationToken token);
        //Task<Result<int>> GetBalanceAsync(CancellationToken token);
        //Task<Result<int>> GetBalanceByIdAsync(long id, CancellationToken token);
        //Task<Result<bool>> ReconcileAccount(long id, CancellationToken token);
        
    }
}
