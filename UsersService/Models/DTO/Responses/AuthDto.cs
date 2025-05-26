namespace UsersService.Models.DTO.Responses
{
    public record AuthDto(
        long UserId,
        string Token);
}
