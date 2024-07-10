using Application.Generic;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using Web.ViewModels;

namespace Application.Repositories
{
    public interface IEventsRepository : IGenericRepository<EventBaseModel, Guid>
    {
        Task<(ICollection<EventBaseModel>, int)> GetFilteredEventsAsync(List<FilterOption> filterOptions,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0);

        Task<EventExtendedModel?> GetExtendedEventByIdAsync(Guid id);

        Task<ICollection<EventBaseModel>> GetMostPopularAsync();

        Task BookTickets(Guid eventId, int bookedTickets);

        Task CancelTickets(Guid eventId, int bookedTickets);
    }
}
