namespace CaregoryAccountService.Models.Errors
{
    public static class AccountErrors
    {
        public static readonly Error AccountNotFound = new("AccountNotFound", "Account not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
