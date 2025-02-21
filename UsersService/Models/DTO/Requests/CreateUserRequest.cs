namespace UsersService.Models.DTO.Requests
{
    public record CreateUserRequest(
        int Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId,
        string Password);
}
