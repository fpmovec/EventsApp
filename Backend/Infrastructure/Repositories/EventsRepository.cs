using Application.CollectionServices;
using Application.Models;
using Application.Repositories;
using Domain.AppSettings;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class EventsBaseRepository : GenericRepository<EventBaseModel, Guid>, IEventsRepository
    {

        public EventsBaseRepository(
            EventContext eventContext,
            ILogger<EventsBaseRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<EventBaseModel> filterService,
            ISortService<EventBaseModel> sortService) 
            : base(eventContext, logger, options, filterService, sortService) { }

        protected override DbSet<EventBaseModel> dbSet
            => _eventContext.Events;

        public async Task<ICollection<EventBaseModel>> GetFilteredEventsAsync(List<FilterOption> filterOptions, SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending, int currentPage = 0)
        {
            var events = await GetAllAsync(filterOptions, sortType, order, currentPage);

            return events.ToList();
        }

        public override Task UpdateAsync(EventBaseModel entity)
        {
            return base.UpdateAsync(entity);
        }
    }
}
