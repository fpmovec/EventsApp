﻿using Domain.Models;
using Web.DTO;

namespace Domain.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking, int>
    {
        Task BookEventAsync(Booking booking, CancellationToken cancellationToken);

        Task CancelBookingAsync(int bookingId, CancellationToken cancellationToken);

        Task<ICollection<UserBrief>> GetEventParticipantsAsync(int eventId, CancellationToken cancellationToken);

        Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId, CancellationToken cancellationToken);

        Task UpdateDependingBookingsAsync(EventExtendedModel eventExtendedModel);
    }
}
