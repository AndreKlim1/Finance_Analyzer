using FluentValidation;
using UsersService.Models.DTO.Requests;
using UsersService.Models.Enums;
using UsersService.Repositories.Interfaces;
using UsersService.Services.Interfaces;
using UsersService.Services.Mappings;

namespace UsersService.Services.Implementations
{
    public class AuthService(IConfiguration config, IUserRepository repository, IValidator<CreateUserRequest> validator) : IAuthService
    {

        public async Task<string?> RegisterAsync(CreateUserRequest userRequest, CancellationToken token)
        {
            var result = await repository.GetByEmailAsync(userRequest.Email, token);
            if (result == null) return null;
            await repository.AddAsync(userRequest.ToUser(), token);
            return TokenGenerator.GenerateToken(userRequest.Email, Enum.Parse<Role>(userRequest.Role), config);
        }

        public async Task<string?> LoginAsync(string email, string passwordHash, CancellationToken token)
        {
            var result = await repository.GetByEmailAsync(email, token);
            if (result == null || result.PasswordHash != passwordHash) return null;
            return TokenGenerator.GenerateToken(email, result.Role, config);
        }
    }
}
