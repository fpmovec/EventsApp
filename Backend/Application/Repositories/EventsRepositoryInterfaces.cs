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
    }
}
