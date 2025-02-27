namespace UsersService.Models.DTO.Requests
{
    public record CreateUserRequest(
        string Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId,
        string PasswordHash);
}
