namespace UsersService.Models.DTO.Responses
{
    public record UserResponse(
        long Id,
        int Role,
        string Email,
        DateTime RegistrationDate,
        long ProfileId);
}
