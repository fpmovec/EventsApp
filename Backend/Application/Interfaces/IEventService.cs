using Application.ViewModels;
using Domain.Models;
using Web.DTO;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<FilteredEventsResponse> GetFilteredEventsAsync(FilterOptionsDTO options, CancellationToken cancellationToken);
        Task CreateEventAsync(EventDTO eventViewModel, string webRootPath, CancellationToken cancellationToken);
        Task<EventExtendedModel> EditEventAsync(int id, EventDTO eventViewModel, string webRootPath, CancellationToken cancellationToken);
        Task DeleteEventByIdAsync(int id, CancellationToken cancellationToken);
        Task<EventExtendedModel> GetEventInfoAsync(int id, CancellationToken cancellationToken);
        Task<ICollection<EventBaseModel>> GetMostPopularAsync(CancellationToken cancellationToken);
        Task BookTickets(int eventId, int bookedTickets, CancellationToken cancellationToken);
        Task CancelTickets(int eventId, int bookedTickets, CancellationToken cancellationToken);
    }
}
