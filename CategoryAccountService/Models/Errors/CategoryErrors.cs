namespace CaregoryAccountService.Models.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error ProfileNotFound = new("ProfileNotFound", "Profile not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
