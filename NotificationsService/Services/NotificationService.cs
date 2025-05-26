using Microsoft.AspNetCore.SignalR;

namespace NotificationsService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task SendToUserAsync(string userId, string message)
        {
            return _hub.Clients.Group(userId)
                              .SendAsync("ReceiveNotification", new { message });
        }
    }
}
