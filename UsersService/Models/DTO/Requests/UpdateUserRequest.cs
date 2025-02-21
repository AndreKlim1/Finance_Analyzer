namespace UsersService.Models.DTO.Requests
{
    public record UpdateUserRequest(
        long Id,
        int Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId,
        string Password);
}
