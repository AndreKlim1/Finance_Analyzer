using System.Runtime.CompilerServices;
using CaregoryAccountService.Models;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Services.Mappings
{
    public static class AccountMapping
    {
        public static AccountResponse ToAccountResponse(this Account account)
        {
            return new AccountResponse
            (
                account.Id,
                account.UserId,
                account.AccountName,
                account.AccountType.ToString(),
                account.Currency.ToString(),
                account.Balance,
                account.TransactionsCount,
                account.Description,
                account.Color
            );
        }

        public static Account ToAccount(this CreateAccountRequest request)
        {
            return new Account
            {
                UserId = request.UserId,
                AccountName = request.AccountName,
                AccountType = Enum.Parse<AccountType>(request.AccountType),
                Currency = Enum.Parse<Currency>(request.Currency),
                Balance = request.Balance,
                TransactionsCount = request.TransactionsCount,
                Description = request.Description,
                Color = request.Color
            };
        }

        public static Account ToAccount(this UpdateAccountRequest request)
        {
            return new Account
            {
                Id = request.Id,
                UserId = request.UserId,
                AccountName = request.AccountName,
                AccountType = Enum.Parse<AccountType>(request.AccountType),
                Currency = Enum.Parse<Currency>(request.Currency),
                Balance = request.Balance,
                TransactionsCount = request.TransactionsCount,
                Description = request.Description,
                Color = request.Color
            };
        }
    }
}
