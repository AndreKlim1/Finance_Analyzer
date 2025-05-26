using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using CategoryAccountService.Messaging.Kafka;
using CategoryAccountService.Messaging.DTO;
using CaregoryAccountService.Messaging.DTO;
using Microsoft.AspNetCore.SignalR;
using CategoryAccountService.Messaging.SignalR;

namespace CaregoryAccountService.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IHubContext<AccountsHub> _hub;
        public AccountService(IAccountRepository accountRepository, IKafkaProducer kafkaProducer, IHubContext<AccountsHub> hub)
        {
            _accountRepository = accountRepository;
            _kafkaProducer = kafkaProducer;
            _hub = hub;
        }

        public async Task<Result<AccountResponse>> GetAccountByIdAsync(long id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id, token);

            return account is null
                ? Result<AccountResponse>.Failure(AccountErrors.AccountNotFound)
                : Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<Result<AccountResponse>> CreateAccountAsync(CreateAccountRequest createAccountRequest,
            CancellationToken token)
        {

            var account = createAccountRequest.ToAccount();
            await _accountRepository.AddAsync(account, token);

            if (account is null)
            {
                return Result<AccountResponse>.Failure(AccountErrors.AccountNotFound);
            }
            else
            {
                if (account.Balance != 0)
                {
                    var updateEvent = new AccountUpdateEvent(account, account.Balance);
                    await _kafkaProducer.ProduceAsync("account-events", account.Id.ToString(), updateEvent);
                }
                await _kafkaProducer.ProduceAsync("notifications-events", account.UserId.ToString(), new NotificationEvent(account.UserId.ToString(), $"Создан счёт {account.AccountName}"));
                return Result<AccountResponse>.Success(account.ToAccountResponse());
            }

        }

        public async Task<Result<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest,
            CancellationToken token)
        {
            var account = updateAccountRequest.ToAccount();
            var prevBalance = await _accountRepository.GetBalanceByIdAsync(account.Id, token);
            account = await _accountRepository.UpdateAsync(account, token);

            if (account is null)
            {
                return Result<AccountResponse>.Failure(AccountErrors.AccountNotFound);
            }
            else
            {
                var value = account.Balance - prevBalance;
                if (value != 0)
                {
                    var updateEvent = new AccountUpdateEvent(account, value.Value);
                    await _kafkaProducer.ProduceAsync("account-events", account.Id.ToString(), updateEvent);
                }
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
                return Result<List<AccountResponse>>.Failure(AccountErrors.AccountNotFound);
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

        public async Task<Result<List<AccountResponse>>> GetAccountsByUserIdAsync(long userId, CancellationToken token)
        {
            var accounts = await _accountRepository.FindByCondition(l => l.UserId == userId, true).ToListAsync();
            if (accounts is null)
            {
                return Result<List<AccountResponse>>.Failure(AccountErrors.AccountNotFound);
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

        public async Task<Result<AccountResponse>> UpdateBalanceAsync(long id, decimal value, int trCountChange, CancellationToken token)
        {
            
            var account = await _accountRepository.GetByIdAsync(id, token);
            account.Balance += value;
            account.TransactionsCount+= trCountChange;
            account = await _accountRepository.UpdateAsync(account, token);
            if (account != null)
            {
                await SendToUserAsync(account.UserId.ToString(), account.ToAccountResponse());
            }
            return account is null
                ? Result<AccountResponse>.Failure(AccountErrors.AccountNotFound)
                : Result<AccountResponse>.Success(account.ToAccountResponse());
        }

        public async Task<Result<string>> GetCurrencyByIdAsync(long id, CancellationToken token)
        {
            var currency = await _accountRepository.GetCurrencyByIdAsync(id, token);
            return currency is null
                ? Result<string>.Failure(AccountErrors.AccountNotFound)
                : Result<string>.Success(currency.ToString());
        }

        public Task SendToUserAsync(string userId, AccountResponse message)
        {
            return _hub.Clients.Group(userId)
                              .SendAsync("AccountBalanceUpdate", new { message });
        }
    }
}
