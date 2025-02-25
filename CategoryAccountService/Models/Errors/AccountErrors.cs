namespace CaregoryAccountService.Models.Errors
{
    public static class AccountErrors
    {
        public static readonly Error UserNotFound = new("UserNotFound", "User not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
