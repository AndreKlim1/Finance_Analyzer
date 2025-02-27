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
        private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserRequestValidator;

        public UserService(IUserRepository userRepository, IValidator<CreateUserRequest> createUserRequestValidator,
            IValidator<UpdateUserRequest> updateUserRequestValidator)
        {
            _userRepository = userRepository;
            _createUserRequestValidator = createUserRequestValidator;
            _updateUserRequestValidator = updateUserRequestValidator;
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
            var validationResult = await _createUserRequestValidator.ValidateAsync(createUserRequest, token);

            if (!validationResult.IsValid)
                return Result<UserResponse>.Failure(UserErrors.InvalidCredentials);

            var user = createUserRequest.ToUser();

            await _userRepository.AddAsync(user, token);

            return Result<UserResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest,
            CancellationToken token)
        {
            var validationResult = await _updateUserRequestValidator.ValidateAsync(updateUserRequest, token);

            if (!validationResult.IsValid)
                return Result<UserResponse>.Failure(UserErrors.InvalidCredentials);

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
