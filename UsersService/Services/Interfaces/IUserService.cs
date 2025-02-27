using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Errors;

namespace UsersService.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponse>> GetUserByIdAsync(long id, CancellationToken token);
        Task<Result<List<UserResponse>>> GetUsersAsync(CancellationToken token);
        Task<Result<UserResponse>> GetUserByEmailAsync(string email, CancellationToken token);
        Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken token);
        Task<Result<UserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken token);
        Task<bool> DeleteUserAsync(long id, CancellationToken token);
    }
}
