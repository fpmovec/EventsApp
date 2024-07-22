using Application.ViewModels;
using Entities.Models;
using Web.ViewModels;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<FilteredEventsResponse?> GetFilteredEventsAsync(FilterOptionsViewModel options, CancellationToken cancellationToken);
        Task CreateEventAsync(EventViewModel eventViewModel, string webRootPath, CancellationToken cancellationToken);
        Task<EventExtendedModel> EditEventAsync(int id, EventViewModel eventViewModel, string webRootPath, CancellationToken cancellationToken);
        Task DeleteEventByIdAsync(int id, CancellationToken cancellationToken);
        Task<EventExtendedModel?> GetEventInfoAsync(int id, CancellationToken cancellationToken);
        Task<ICollection<EventBaseModel>> GetMostPopularAsync(CancellationToken cancellationToken);
        Task BookTickets(int eventId, int bookedTickets, CancellationToken cancellationToken);
        Task CancelTickets(int eventId, int bookedTickets, CancellationToken cancellationToken);
    }
}
