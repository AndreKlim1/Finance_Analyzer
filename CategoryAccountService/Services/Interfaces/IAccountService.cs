using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;

namespace CaregoryAccountService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountResponse>> GetUserByIdAsync(long id, CancellationToken token);
        Task<Result<AccountResponse>> GetUserByEmailAsync(string email, CancellationToken token);
        Task<Result<AccountResponse>> CreateUserAsync(CreateAccountRequest createUserRequest, CancellationToken token);
        Task<Result<AccountResponse>> UpdateUserAsync(UpdateAccountRequest updateUserRequest, CancellationToken token);
        Task<bool> DeleteUserAsync(long id, CancellationToken token);
    }
}
