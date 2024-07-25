using Application.CollectionServices;
using Domain.Repositories;
using Domain.AppSettings;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.ViewModels;

namespace Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking, int>, IBookingRepository
    {
        public BookingRepository(
            EventContext eventContext,
            ILogger<BookingRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<Booking> filterService,
            ISortService<Booking> sortService)
            : base(eventContext, logger, options, filterService, sortService)
        { }

        protected override DbSet<Booking> dbSet => _eventContext.Bookings;

        public async Task BookEventAsync(Booking booking, CancellationToken cancellationToken)
            => await AddAsync(booking, cancellationToken);


        public async Task CancelBookingAsync(int bookingId, CancellationToken cancellationToken)
            => await DeleteByIdAsync(bookingId, cancellationToken);

        public async Task<ICollection<UserBrief>> GetEventParticipantsAsync(int eventId, CancellationToken cancellationToken)
        {
            var users = await dbSet.AsNoTracking()
                .Where(b => b.EventId == eventId)
                .Select(b => new UserBrief
                {
                    Id = b.UserId,
                    FullName = b.FullName,
                    Email = b.Email,
                    Phone = b.Phone,
                    Birthday = b.Birthday,
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            return users;
        }

        public async Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId, CancellationToken cancellationToken)
        {
            return await dbSet.AsNoTracking()
                 .Where(b => b.UserId == userId)
                 .ToListAsync(cancellationToken);
        }

        public async Task UpdateDependingBookingsAsync(EventExtendedModel eventExtendedModel)
        {
            IQueryable<Booking> eventBookings = dbSet.Where(b => b.EventId == eventExtendedModel.Id);

            foreach(Booking booking in eventBookings)
            {
                booking.EventName = eventExtendedModel.Name;
                booking.PricePerOne = eventExtendedModel.Price;
            }

            dbSet.UpdateRange(eventBookings);

            await Task.CompletedTask;
        }

    }
}
