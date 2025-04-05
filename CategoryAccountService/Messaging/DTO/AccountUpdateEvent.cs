using CaregoryAccountService.Models;
using System.Security.Cryptography.X509Certificates;

namespace CategoryAccountService.Messaging.DTO
{
    public class AccountUpdateEvent
    {
        public AccountUpdateEvent(Account account, decimal value)
        {
            Id = account.Id;
            UserId = account.UserId;
            Currency = account.Currency.ToString();
            AccountName = account.AccountName;
            Value = value;
        }
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }

    }
}
