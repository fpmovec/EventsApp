using Application.CollectionServices;
using Application.Repositories;
using Domain.AppSettings;
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

        protected override DbSet<EventBaseModel> dbSet => _eventContext.Events;

        public override Task UpdateAsync(EventBaseModel entity)
        {
            return base.UpdateAsync(entity);
        }
    }
}
