namespace UsersService.Models.DTO.Requests
{
    public record LoginUserRequest(
    string Email,
    string PasswordHash);
}
