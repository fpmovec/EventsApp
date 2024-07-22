using Entities.Enums;
using Entities.Models;

namespace Domain.Repositories
{
    public interface ICategoryRepository : IGenericRepository<EventCategory, int>
    {
        public Task<IQueryable<EventCategory>> GetAllCategoriesAsync(SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending);

        public Task<EventCategory?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken);
    }
}
