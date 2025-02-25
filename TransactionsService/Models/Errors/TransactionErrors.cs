namespace TransactionsService.Models.Errors
{
    public static class TransactionErrors
    {
        public static readonly Error TransactionNotFound = new("TransactionNotFound", "Transaction not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
