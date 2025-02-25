using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Interfaces;

namespace CaregoryAccountService.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _userRepository;
        private readonly IValidator<CreateAccountRequest> _createUserRequestValidator;
        private readonly IValidator<UpdateAccountRequest> _updateUserRequestValidator;

        public AccountService(IAccountRepository userRepository, IValidator<CreateAccountRequest> createUserRequestValidator,
            IValidator<UpdateAccountRequest> updateUserRequestValidator)
        {
            _userRepository = userRepository;
            _createUserRequestValidator = createUserRequestValidator;
            _updateUserRequestValidator = updateUserRequestValidator;
        }

        public async Task<Result<AccountResponse>> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = await _userRepository.GetByIdAsync(id, token);

            return user is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<AccountResponse>> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var user = await _userRepository.GetByEmailAsync(email, token);

            return user is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<AccountResponse>> CreateUserAsync(CreateAccountRequest createUserRequest,
            CancellationToken token)
        {
            var validationResult = await _createUserRequestValidator.ValidateAsync(createUserRequest, token);

            if (!validationResult.IsValid)
                return Result<AccountResponse>.Failure(AccountErrors.InvalidCredentials);

            var user = createUserRequest.ToUser();

            await _userRepository.AddAsync(user, token);

            return Result<AccountResponse>.Success(user.ToUserResponse());
        }

        public async Task<Result<AccountResponse>> UpdateUserAsync(UpdateAccountRequest updateUserRequest,
            CancellationToken token)
        {
            var validationResult = await _updateUserRequestValidator.ValidateAsync(updateUserRequest, token);

            if (!validationResult.IsValid)
                return Result<AccountResponse>.Failure(AccountErrors.InvalidCredentials);

            var user = updateUserRequest.ToUser();
            user = await _userRepository.UpdateAsync(user, token);

            return user is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(user.ToUserResponse());
        }

        public async Task<bool> DeleteUserAsync(long id, CancellationToken token)
        {
            await _userRepository.DeleteAsync(id, token);

            return true;
        }
    }
}
