using Domain.Models;

namespace Application
{
    public interface INotificationsClient
    {
        Task ReceiveMessageAsync(DetailsChangedEvent @event);
    }
}
