namespace NotificationsService.Services
{
    public interface INotificationService
    {
        Task SendToUserAsync(string userId, string message);
    }
}
