using Application.Generic;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using Web.ViewModels;

namespace Application.Repositories
{
    public interface IEventsRepository : IGenericRepository<EventBaseModel, int>
    {
        Task<(ICollection<EventBaseModel>, int)> GetFilteredEventsAsync(List<FilterOption> filterOptions,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0);

        Task<EventExtendedModel?> GetExtendedEventByIdAsync(int id);

        Task<ICollection<EventBaseModel>> GetMostPopularAsync();

        Task BookTickets(int eventId, int bookedTickets);

        Task CancelTickets(int eventId, int bookedTickets);
    }
}
