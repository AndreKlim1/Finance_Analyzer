namespace UsersService.Models.DTO.Requests
{
    public record UpdateProfileRequest(
        long Id,
        string FirstName,
        string LastName);
}
