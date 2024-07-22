using Application.CollectionServices;
using Application.Services;
using Domain.Repositories;
using Entities.AppSettings;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class EventsBaseRepository : GenericRepository<EventBaseModel, int>, IEventsRepository
    {
        public EventsBaseRepository(
            EventContext eventContext,
            ILogger<EventsBaseRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<EventBaseModel> filterService,
            ISortService<EventBaseModel> sortService)
            : base(eventContext, logger, options, filterService, sortService)
        { }

        protected override DbSet<EventBaseModel> dbSet
            => _eventContext.Events;

        private DbSet<EventExtendedModel> dbSetExtended
            => _eventContext.ExtendedEvents;

        public async Task<(ICollection<EventBaseModel>, int)> GetFilteredEventsAsync(List<FilterOption> filterOptions, SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending, int currentPage = 0)
        {
            var events = await GetAllAsync(filterOptions, sortType, order, currentPage);

            return (events.Item1.ToList(), events.Item2);
        }

        public async Task<EventExtendedModel?> GetExtendedEventByIdAsync(int id, CancellationToken cancellationToken)
        {
            EventExtendedModel? eventExtended = await dbSetExtended
                .Include(e => e.Image)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return eventExtended;
        }

        public async Task<ICollection<EventBaseModel>> GetMostPopularAsync(CancellationToken cancellationToken)
        {
            var popularEvents = await dbSetExtended
                .AsNoTracking()
                .Include(e => e.Image)
                .Include(e => e.Category)
                .OrderByDescending(e => e.BookedTicketsCount)
                .ThenBy(e => e.Date)
                .Where(e => e.Date > DateTime.UtcNow)
                .Take(5)
                .Select(e => (EventBaseModel)e)
                .ToListAsync(cancellationToken);

            return popularEvents;
        }
    }
}
