namespace BudgetingService.Models.Errors
{
    public static class BudgetErrors
    {
        public static readonly Error TransactionNotFound = new("TransactionNotFound", "Transaction not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
