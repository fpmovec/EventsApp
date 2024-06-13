using Application.CollectionServices;
using Application.Generic;
using Domain.AppSettings;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
        where TEntity : class
    {
        protected readonly EventContext _eventContext;
        protected readonly ILogger<GenericRepository<TEntity, TId>> _logger;
        protected readonly PaginationSettings _settings;
        protected readonly IFilterService<TEntity> _filterService;
        protected readonly ISortService<TEntity> _sortService;

        protected GenericRepository(EventContext eventContext,
            ILogger<GenericRepository<TEntity, TId>> logger,
            IOptions<AppSettings> options,
            IFilterService<TEntity> filterService,
            ISortService<TEntity> sortService)
        {
            _eventContext = eventContext;
            _logger = logger;
            _settings = options.Value.PaginationSettings;
            _filterService = filterService;
            _sortService = sortService;
        }

        protected abstract DbSet<TEntity> dbSet { get; }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);

            _logger.LogInformation("Entity has been added");
        }

        public async Task DeleteByIdAsync(TId id)
        {
            TEntity? entity = await dbSet.FindAsync(id);

            if (entity is not null)
            {
                dbSet.Remove(entity);

                _logger.LogInformation("Entity has been deleted from DB");
            }

            _logger.LogWarning("Entity hasn't been deleted because of the null value");
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync(
            FilterType filterType = FilterType.Default,
            object filterValue = null,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0)
        {
            IQueryable<TEntity> entities = dbSet;

            if (filterType is not FilterType.Default && filterValue is not null)
            {
               entities = _filterService.Filter(entities, filterType, filterValue);

                _logger.LogInformation("Collection has been filtered");
            }

            entities = _sortService.Sort(entities, sortType, order);

            _logger.LogInformation("Collection has been sorted");

            if (currentPage is not 0)
            {
                entities = SelectItemsForPage(entities, currentPage);
            }
            await Task.CompletedTask;
            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
            => await dbSet.FindAsync(id);

        public async virtual Task UpdateAsync(TEntity entity)
        {
           dbSet.Update(entity);

            _logger.LogInformation("Entity has been updated");

           await Task.CompletedTask;
        }

        private IQueryable<TEntity> SelectItemsForPage(IQueryable<TEntity> collection, int currentPage)
        {
            return collection.Skip(_settings.PageSize * (currentPage - 1))
                .Take(_settings.PageSize);
        }
    }
}
