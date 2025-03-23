using System.Security.Cryptography.X509Certificates;

namespace TransactionsService.Messaging.Events
{
    public class AccountUpdateEvent
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Value { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }

    }
}
