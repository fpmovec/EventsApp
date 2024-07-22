using Application;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class NotificationsHub(ILogger<NotificationsHub> logger) : Hub<INotificationsClient>
    {
        public async Task SendMessage(DetailsChangedEvent @event)
        {
            await Clients.All.ReceiveMessageAsync(@event);

            logger.LogInformation("Message was sent");
        }
    }
}
