using System.Data;
using TransactionsService.Models.Enums;


namespace TransactionsService.Models
{
    public class Transaction : BaseModel<long>
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
        public long CategoryId { get; set; }
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreationDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Merchant { get; set; } 

    }
}
