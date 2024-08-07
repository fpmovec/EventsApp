﻿using Application.CollectionServices;
using Domain.Repositories;
using Domain.AppSettings;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(entity, cancellationToken);

            _logger.LogInformation("Entity has been added");
        }

        public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken)
        {
            TEntity? entity = await dbSet.FindAsync(id, cancellationToken);

           dbSet.Remove(entity);

            _logger.LogWarning("Entity hasn't been deleted because of the null value");
        }

        public async Task<(IQueryable<TEntity>, int)> GetAllAsync(
            List<FilterOption> filterOptions,
            SortType sortType = SortType.Default,
            SortOrder order = SortOrder.Ascending,
            int currentPage = 0)
        {
            IQueryable<TEntity> entities = dbSet;

            entities = _filterService.FilterWithManyOptions(entities, filterOptions);

            _logger.LogInformation("Collection has been filtered");


            entities = _sortService.Sort(entities, sortType, order);

            _logger.LogInformation("Collection has been sorted");

            (IQueryable<TEntity> ent, int pages)  = (entities, 0);

            if (currentPage is not 0)
            {
                (ent, pages) = SelectItemsForPage(entities, currentPage);
            }
            await Task.CompletedTask;
            return (ent, pages);
        }

        public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
            => await dbSet.FindAsync(id, cancellationToken);

        public async virtual Task UpdateAsync(TEntity entity)
        {
           dbSet.Update(entity);

            _logger.LogInformation("Entity has been updated");

           await Task.CompletedTask;
        }

        private (IQueryable<TEntity>, int) SelectItemsForPage(IQueryable<TEntity> collection, int currentPage)
        {
            return (collection
                .Skip(_settings.PageSize * (currentPage - 1))
                .Take(_settings.PageSize), (int)Math.Ceiling((double)collection.Count() / _settings.PageSize));
        }
    }
}
