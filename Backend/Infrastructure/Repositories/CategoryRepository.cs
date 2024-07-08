using Application.CollectionServices;
using Application.Repositories;
using Domain.AppSettings;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<EventCategory, int>, ICategoryRepository
    {
        public CategoryRepository(
            EventContext eventContext,
            ILogger<CategoryRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<EventCategory> filterService,
            ISortService<EventCategory> sortService)
            : base(eventContext, logger, options, filterService, sortService) { }

        protected override DbSet<EventCategory> dbSet => _eventContext.Categories;

        public async Task<ICollection<EventCategory>> GetAllCategoriesAsync(SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending)
        {
            return (await GetAllAsync(filterOptions: new(), sortType: sortType, order: order)).Item1.ToList();
        }

        public async Task<EventCategory?> GetCategoryByName(string name)
            => await dbSet.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
    }
}
