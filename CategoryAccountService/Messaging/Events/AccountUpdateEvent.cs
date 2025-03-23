using CaregoryAccountService.Models;
using System.Security.Cryptography.X509Certificates;

namespace CategoryAccountService.Messaging.Events
{
    public class AccountUpdateEvent
    {
        public AccountUpdateEvent(Account account, int value) 
        {
            Id = account.Id;
            UserId = account.UserId;
            Currency = account.Currency.ToString();
            AccountName = account.AccountName;
            Value = value;
        }
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Value { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }

    }
}
