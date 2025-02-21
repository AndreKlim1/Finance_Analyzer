namespace UsersService.Models.Errors
{
    public static class ProfileErrors
    {
        public static readonly Error ProfileNotFound = new("ProfileNotFound", "Profile not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
