using Application.Generic;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Web.ViewModels;

namespace Application.Repositories
{
   public interface IBookingRepository : IGenericRepository<Booking, int>
   {
        Task BookEventAsync(Booking booking);

        Task CancelBooking(int bookingId);

        Task<ICollection<UserBrief>> GetEventParticipants(int eventId);

        Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId);

        Task UpdateDependingBookingsAsync(EventExtendedModel eventExtendedModel);
   }
}
