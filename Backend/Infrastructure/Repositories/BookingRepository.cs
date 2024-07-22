﻿using Application.CollectionServices;
using Domain.Repositories;
using Entities.AppSettings;
using Entities.Models;
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

        public async Task BookEventAsync(Booking booking)
            => await AddAsync(booking);


        public async Task CancelBooking(int bookingId)
            => await DeleteByIdAsync(bookingId);

        public async Task<ICollection<UserBrief>> GetEventParticipants(int eventId)
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
                .ToListAsync();

            return users;
        }

        public async Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId)
        {
            return await dbSet.AsNoTracking()
                 .Where(b => b.UserId == userId)
                 .ToListAsync();
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
