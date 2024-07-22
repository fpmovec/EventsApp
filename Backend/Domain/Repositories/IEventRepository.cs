using Entities.Enums;
using Entities.Models;

namespace Domain.Repositories
{
    public interface IEventsRepository : IGenericRepository<EventBaseModel, int>
    {
        Task<(ICollection<EventBaseModel>, int)> GetFilteredEventsAsync(List<FilterOption> filterOptions,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0);

        Task<EventExtendedModel?> GetExtendedEventByIdAsync(int id, CancellationToken cancellationToken);

        Task<ICollection<EventBaseModel>> GetMostPopularAsync(CancellationToken cancellationToken);
    }
}
