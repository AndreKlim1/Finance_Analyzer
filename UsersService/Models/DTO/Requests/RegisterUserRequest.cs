namespace UsersService.Models.DTO.Requests
{
    public record RegisterUserRequest
    (
        string Role,
        string Email,
        DateTime RegistrationDate,
        string PasswordHash,
        string FirstName,
        string LastName
    );
}
