using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;

namespace UsersService.Services.Interfaces
{
    public interface IAuthService
    {

        public Task<AuthDto?> RegisterAsync(CreateUserRequest userRequest, CreateProfileRequest profileRequest, CancellationToken token);

        public Task<AuthDto?> LoginAsync(string email, string passwordHash, CancellationToken token);
    }
}
