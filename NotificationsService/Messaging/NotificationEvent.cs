namespace NotificationsService.Messaging
{
    public record NotificationEvent(
        string UserId, 
        string Message);

}
