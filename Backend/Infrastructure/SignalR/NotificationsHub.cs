using Application;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SignalR
{
    public class NotificationsHub(ILogger<NotificationsHub> logger) : Hub<INotificationsClient>
    {
        public async Task SendMessage(DetailsChangedEvent @event)
        {
            await Clients.All.ReceiveMessageAsync(@event);
        }
    }
}
