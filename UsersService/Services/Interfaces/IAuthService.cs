using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Interfaces
{
    public interface IAuthService
    {

        public Task<string?> RegisterAsync(CreateUserRequest userRequest, CreateProfileRequest profileRequest, CancellationToken token);

        public Task<string?> LoginAsync(string email, string passwordHash, CancellationToken token);
    }
}
