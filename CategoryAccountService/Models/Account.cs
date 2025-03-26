using System.Data;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models
{
    public class Account : BaseModel<long>
    {
        public long UserId { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public Currency Currency { get; set; }
        public int Balance { get; set; }
        public int TransactionsCount { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
