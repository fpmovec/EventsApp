using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SignalR
{
    public class NotificationsHub(ILogger<NotificationsHub> logger) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            logger.LogInformation("Connected!");
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
