namespace UsersService.Models.DTO.Responses
{
    public record UserResponse(
        long Id,
        string Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId);
}
