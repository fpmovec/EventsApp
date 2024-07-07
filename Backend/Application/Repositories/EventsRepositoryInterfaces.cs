using Application.Generic;
using Application.Models;
using Domain.Enums;
using Domain.Models;

namespace Application.Repositories
{
    public interface IEventsRepository : IGenericRepository<EventBaseModel, Guid>
    {
        Task<ICollection<EventBaseModel>> GetFilteredEventsAsync(List<FilterOption> filterOptions,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0);

        //Task<ICollection<EventBaseModel>> GetEventsByParticipantIdAsync(Guid id);
        Task<EventExtendedModel?> GetExtendedEventByIdAsync(Guid id);

        Task<ICollection<EventBaseModel>> GetMostPopularAsync();

        Task<int> GetPagesCountAsync();

        Task BookTickets(Guid eventId, int bookedTickets);

        Task CancelTickets(Guid eventId, int bookedTickets);
    }
}
