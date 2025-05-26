namespace CaregoryAccountService.Messaging.DTO
{
    public record NotificationEvent(
        string UserId,
        string Message);
}
