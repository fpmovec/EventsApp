using Application.Generic;
using Domain.Enums;
using Domain.Models;

namespace Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<EventCategory, int>
    {
        public Task<IQueryable<EventCategory>> GetAllCategoriesAsync(SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending);

        public Task<EventCategory?> GetCategoryByName(string name);
    }
}
