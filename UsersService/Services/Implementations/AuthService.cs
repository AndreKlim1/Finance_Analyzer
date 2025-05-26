using FluentValidation;
using UsersService.Models;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Enums;
using UsersService.Repositories.Interfaces;
using UsersService.Services.Interfaces;
using UsersService.Services.Mappings;

namespace UsersService.Services.Implementations
{
    public class AuthService(IConfiguration config, IUserRepository userRepository, IProfileRepository profileRepository) : IAuthService
    {

        public async Task<AuthDto?> RegisterAsync(CreateUserRequest userRequest, CreateProfileRequest profileRequest, CancellationToken token)
        {
            var result = await userRepository.GetByEmailAsync(userRequest.Email, token);
            if (result != null) return null;
            var profile = profileRequest.ToProfile();
            await profileRepository.AddAsync(profile, token);

            var user = userRequest.ToUser();
            user.ProfileId = profile.Id;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = hashedPassword;
            await userRepository.AddAsync(user, token);
            return new AuthDto(user.Id, TokenGenerator.GenerateToken(userRequest.Email, Enum.Parse<Role>(userRequest.Role), user.Id, config));
        }

        public async Task<AuthDto?> LoginAsync(string email, string passwordHash, CancellationToken token)
        {
            var result = await userRepository.GetByEmailAsync(email, token);
            if(result == null) return null;
            result.PasswordHash = result.PasswordHash.Substring(0, 60);
            if (!BCrypt.Net.BCrypt.Verify(passwordHash, result.PasswordHash)) return null;
            return new AuthDto(result.Id, TokenGenerator.GenerateToken(email, result.Role, result.Id, config));
        }

    }
}
