namespace UsersService.Models.DTO.Requests
{
    public record UpdateUserRequest(
        long Id,
        string Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId,
        string Password);
}
