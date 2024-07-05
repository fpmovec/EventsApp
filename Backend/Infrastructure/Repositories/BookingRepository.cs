using Application.CollectionServices;
using Application.Repositories;
using Domain.AppSettings;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
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

        public async Task BookEventAsync(Booking booking)
            => await AddAsync(booking);


        public async Task CancelBooking(int bookingId)
            => await DeleteByIdAsync(bookingId);

        public async Task<ICollection<UserBrief>> GetEventParticipants(Guid eventId)
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
                }).ToListAsync();

            return users;
        }
    }
}
