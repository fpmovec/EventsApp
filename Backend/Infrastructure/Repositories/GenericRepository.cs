using Application.CollectionServices;
using Application.Generic;
using Domain.AppSettings;
using Domain.Enums;
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

        public IQueryable<TEntity> GetAllAsync(
            EventFilterType? filterType = null, object filterValue = null, EventsSortType sortType = EventsSortType.Default, SortOrder order = SortOrder.Ascending, int currentPage = 1)
        {
            IQueryable<TEntity> events = dbSet;

            if (filterType is not null && filterValue is not null)
            {
               events = _filterService.Filter(events, filterType, filterValue);
            }

            events = _sortService.Sort(events, sortType, order);

            events = SelectItemsForPage(events, currentPage);
            
            return events;
        }

        public TEntity GetByIdAsync(TId id)
        {
            throw new NotImplementedException();
        }

        private IQueryable<TEntity> SelectItemsForPage(IQueryable<TEntity> collection, int currentPage)
        {
            return collection.Skip(_settings.PageSize * (currentPage - 1))
                .Take(_settings.PageSize);
        }
    }
}
