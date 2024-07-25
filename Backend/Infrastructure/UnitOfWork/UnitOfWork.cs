using Application.Services;
using Domain.Repositories;
using Domain.UnitOfWork;
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
            IBookingRepository bookingRepository,
            IJwtRepository jwtRepository)
        {
            _eventContext = eventContext;
            _logger = logger;
            EventsRepository = eventsRepository;
            CategoryRepository = categoryRepository;
            BookingRepository = bookingRepository;
            JwtRepository = jwtRepository;
        }

        public IEventsRepository EventsRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public IBookingRepository BookingRepository { get; private set; }

        public IJwtRepository JwtRepository { get; private set; }

        public async Task CompleteAsync(CancellationToken cancellationToken)
        {
            await _eventContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Changes have been saved!");
        }

        public void Dispose()
        {
            _eventContext.Dispose();
        }
    }
}
