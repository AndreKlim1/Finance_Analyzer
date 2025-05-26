using Microsoft.AspNetCore.SignalR;

namespace NotificationsService
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            var uid = http.Request.Query["userId"];
            if (!string.IsNullOrEmpty(uid))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, uid);
            }
            await base.OnConnectedAsync();
        }
    }
}
