using Application.Repositories;
using Application.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EventContext _eventContext;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(
            EventContext eventContext,
            ILogger<UnitOfWork> logger,
            IEventsRepository eventsRepository,
            ICategoryRepository categoryRepository,
            IBookingRepository bookingRepository)
        {
            _eventContext = eventContext;
            _logger = logger;
            EventsRepository = eventsRepository;
            CategoryRepository = categoryRepository;
            BookingRepository = bookingRepository;
        }

        public IEventsRepository EventsRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public IBookingRepository BookingRepository { get; private set; }
        public async Task CompleteAsync()
        {
            await _eventContext.SaveChangesAsync();
            _logger.LogInformation("Changes have been saved!");
        }

        public void Dispose()
        {
            _eventContext.Dispose();
        }
    }
}
