using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace CaregoryAccountService.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<CreateAccountRequest> _createAccountRequestValidator;
        private readonly IValidator<UpdateAccountRequest> _updateAccountRequestValidator;

        public AccountService(IAccountRepository accountRepository, IValidator<CreateAccountRequest> createAccountRequestValidator,
            IValidator<UpdateAccountRequest> updateAccountRequestValidator)
        {
            _accountRepository = accountRepository;
            _createAccountRequestValidator = createAccountRequestValidator;
            _updateAccountRequestValidator = updateAccountRequestValidator;
        }

        public async Task<Result<AccountResponse>> GetAccountByIdAsync(long id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id, token);

            return account is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<Result<AccountResponse>> CreateAccountAsync(CreateAccountRequest createAccountRequest,
            CancellationToken token)
        {
            var validationResult = await _createAccountRequestValidator.ValidateAsync(createAccountRequest, token);

            if (!validationResult.IsValid)
                return Result<AccountResponse>.Failure(AccountErrors.InvalidCredentials);

            var account = createAccountRequest.ToAccount();

            await _accountRepository.AddAsync(account, token);

            return Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<Result<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest,
            CancellationToken token)
        {
            var validationResult = await _updateAccountRequestValidator.ValidateAsync(updateAccountRequest, token);

            if (!validationResult.IsValid)
                return Result<AccountResponse>.Failure(AccountErrors.InvalidCredentials);

            var account = updateAccountRequest.ToAccount();
            account = await _accountRepository.UpdateAsync(account, token);

            return account is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<bool> DeleteAccountAsync(long id, CancellationToken token)
        {
            await _accountRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<List<AccountResponse>>> GetAccountsAsync(CancellationToken token)
        {
            var accounts = await _accountRepository.FindAll(true).ToListAsync();
            if (accounts is null)
            {
                return Result<List<AccountResponse>>.Failure(AccountErrors.UserNotFound);
            }
            else
            {
                var responses = new List<AccountResponse>();
                foreach (var account in accounts)
                {
                    responses.Add(account.ToAccountResponse());
                }
                return Result<List<AccountResponse>>.Success(responses);
            }
        }

    }
}
