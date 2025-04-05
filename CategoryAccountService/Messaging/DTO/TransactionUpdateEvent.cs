namespace CategoryAccountService.Messaging.DTO
{
    public class TransactionUpdateEvent
    {
            public TransactionUpdateEvent(TransactionData prevTransaction, TransactionData currTransaction)
            {
                PrevTransaction = prevTransaction;
                CurrTransaction = currTransaction;
            }
            public TransactionData PrevTransaction { get; set; }
            public TransactionData CurrTransaction { get; set; }
        
    }
}
