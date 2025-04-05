namespace CaregoryAccountService.Models.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error CategoryNotFound = new("CategoryNotFound", "Category not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
