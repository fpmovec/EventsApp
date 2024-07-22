﻿using Application.CollectionServices;
using Domain.Repositories;
using Entities.AppSettings;
using Entities.Enums;
using Entities.Models;
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

        public async Task<IQueryable<EventCategory>> GetAllCategoriesAsync(SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending)
        {
            var categories = await GetAllAsync(filterOptions: new(), sortType: sortType, order: order);
            return categories.Item1;
        }

        public async Task<EventCategory?> GetCategoryByName(string name)
            => await dbSet.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
    }
}
