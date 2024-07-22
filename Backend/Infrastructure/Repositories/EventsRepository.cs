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
        private readonly PaginationSettings paginationSettings;
        public EventsBaseRepository(
            EventContext eventContext,
            ILogger<EventsBaseRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<EventBaseModel> filterService,
            ISortService<EventBaseModel> sortService)
            : base(eventContext, logger, options, filterService, sortService)
        {
            paginationSettings = options.Value.PaginationSettings;
        }

        protected override DbSet<EventBaseModel> dbSet
            => _eventContext.Events;

        private DbSet<EventExtendedModel> dbSetExtended
            => _eventContext.ExtendedEvents;

        public async Task<(ICollection<EventBaseModel>, int)> GetFilteredEventsAsync(List<FilterOption> filterOptions, SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending, int currentPage = 0)
        {
            var events = await GetAllAsync(filterOptions, sortType, order, currentPage);

            return (events.Item1.ToList(), events.Item2);
        }

        public async Task<EventExtendedModel?> GetExtendedEventByIdAsync(int id)
        {
            EventExtendedModel? eventExtended = await dbSetExtended
                .Include(e => e.Image)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return eventExtended;
        }

        public async Task BookTickets(int eventId, int bookedTickets)
        {
            EventExtendedModel extendedEvent = await dbSetExtended.FindAsync(eventId);

            extendedEvent.BookedTicketsCount += bookedTickets;
        }

        public async Task<ICollection<EventBaseModel>> GetMostPopularAsync()
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
                .ToListAsync();

            return popularEvents;
        }

        public async override Task UpdateAsync(EventBaseModel entity)
        {
            await base.UpdateAsync(entity);
        }

        public async Task CancelTickets(int eventId, int bookedTickets)
        {
            EventExtendedModel extendedEvent = await dbSetExtended.FindAsync(eventId);

            extendedEvent.BookedTicketsCount -= bookedTickets;
        }
    }
}
