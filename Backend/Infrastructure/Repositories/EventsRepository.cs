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

        private DbSet<EventExtendedModel> dbSetExtended
            => _eventContext.ExtendedEvents;

        public async Task<ICollection<EventBaseModel>> GetEventsByParticipantId(Guid id)
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

        public async Task<EventExtendedModel?> GetExtendedEventById(Guid id)
        {
            EventExtendedModel? eventExtended = await dbSetExtended
                .Include(e => e.Image)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return eventExtended;
        }

        public override async Task UpdateAsync(EventBaseModel entity)
        {
            await base.UpdateAsync(entity);
        }
    }
}
