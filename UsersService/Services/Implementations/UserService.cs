using FluentValidation;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Services.Interfaces;
using UsersService.Services.Mappings;
using UsersService.Models.Errors;
using UsersService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UsersService.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = await _userRepository.GetByIdAsync(id, token);

            return user is null
                ? Result<UserResponse>.Failure(UserErrors.UserNotFound)
                : Result<UserResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<UserResponse>> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var user = await _userRepository.GetByEmailAsync(email, token);

            return user is null
                ? Result<UserResponse>.Failure(UserErrors.UserNotFound)
                : Result<UserResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest,
            CancellationToken token)
        {

            var user = createUserRequest.ToUser();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = hashedPassword;
            await _userRepository.AddAsync(user, token);

            return Result<UserResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest,
            CancellationToken token)
        {
            
            var user = updateUserRequest.ToUser();
            user = await _userRepository.UpdateAsync(user, token);

            return user is null
                ? Result<UserResponse>.Failure(UserErrors.UserNotFound)
                : Result<UserResponse>.Success(user.ToUserResponse());
        }

        public async Task<bool> DeleteUserAsync(long id, CancellationToken token)
        {
            await _userRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<List<UserResponse>>> GetUsersAsync(CancellationToken token)
        {
            var users = await _userRepository.FindAll(true).ToListAsync();

            if(users is null)
            {
                return Result<List<UserResponse>>.Failure(UserErrors.UserNotFound);
            }
            else
            {
                var responses = new List<UserResponse>();
                foreach (var user in users)
                {
                    responses.Add(user.ToUserResponse());
                }
                return Result<List<UserResponse>>.Success(responses);
            }
        }
    }
}
