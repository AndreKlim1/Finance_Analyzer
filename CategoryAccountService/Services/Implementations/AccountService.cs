using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using CategoryAccountService.Messaging;
using CategoryAccountService.Messaging.Events;

namespace CaregoryAccountService.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public AccountService(IAccountRepository accountRepository, IKafkaProducer kafkaProducer)
        {
            _accountRepository = accountRepository;
            _kafkaProducer = kafkaProducer;
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

            var account = createAccountRequest.ToAccount();

            await _accountRepository.AddAsync(account, token);

            return Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<Result<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest,
            CancellationToken token)
        {
            var account = updateAccountRequest.ToAccount();
            var prevBalance = await _accountRepository.GetBalanceByIdAsync(account.Id, token);
            account = await _accountRepository.UpdateAsync(account, token);

            if (account is null)
            {
                return Result<AccountResponse>.Failure(AccountErrors.UserNotFound);
            }
            else
            {
                var value = account.Balance - prevBalance;
                var updateEvent = new AccountUpdateEvent(account, value.Value);
                await _kafkaProducer.ProduceAsync("account-events", account.Id.ToString(), updateEvent);
                return Result<AccountResponse>.Success(account.ToAccountResponse());
            }
        }

        public async Task<bool> DeleteAccountAsync(long id, CancellationToken token)
        {
            await _accountRepository.DeleteAsync(id, token);
            var deletionEvent = new DeletionEvent("account-deleted", id);
            await _kafkaProducer.ProduceAsync("deletion-events", id.ToString(), deletionEvent);
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

        public async Task<Result<AccountResponse>> UpdateBalanceAsync(long id, int value, int trCountChange, CancellationToken token)
        {
            
            var account = await _accountRepository.GetByIdAsync(id, token);
            account.Balance += value;
            account.TransactionsCount+= trCountChange;
            account = await _accountRepository.UpdateAsync(account, token);

            return account is null
                ? Result<AccountResponse>.Failure(AccountErrors.UserNotFound)
                : Result<AccountResponse>.Success(account.ToAccountResponse());
        }
    }
}
