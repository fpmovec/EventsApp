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

        public async Task<ICollection<EventBaseModel>> GetEventsByParticipantIdAsync(Guid id)
        {
            return await dbSetExtended.Where(e => e.Participants.Any(c => c.Id == id))
                .Include(e => e.Category)
                .Include(e => e.Image)
                .Select(e => (EventBaseModel)e)
                .ToListAsync();
        }

        public async Task<ICollection<EventBaseModel>> GetFilteredEventsAsync(List<FilterOption> filterOptions, SortType sortType = SortType.Default, SortOrder order = SortOrder.Ascending, int currentPage = 0)
        {
            
            var events = await GetAllAsync(filterOptions, sortType, order, currentPage);

            return events.ToList();
        }

        public async Task<EventExtendedModel?> GetExtendedEventByIdAsync(Guid id)
        {
            EventExtendedModel? eventExtended = await dbSetExtended
                .Include(e => e.Image)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return eventExtended;
        }

        public async Task<ICollection<EventBaseModel>> GetMostPopularAsync()
        {
            var popularEvents = await dbSetExtended
                .AsNoTracking()
                .Include(e => e.Image)
                .Include(e => e.Category)
                .OrderByDescending(e => e.BookedTicketsCount)
                .ThenBy(e => e.Date)
                //.Where(e => e.MaxParticipantsCount != e.BookedTicketsCount)
                //.Where(e => e.Date > DateTime.UtcNow)
                .Take(5)
                .Select(e => (EventBaseModel)e)
                .ToListAsync();

            return popularEvents;
        }

        public override async Task UpdateAsync(EventBaseModel entity)
        {
            await base.UpdateAsync(entity);
        }

        public async Task<int> GetPagesCountAsync() 
            => await Task.FromResult(dbSet.Count() / paginationSettings.PageSize + 1);
    }
}
