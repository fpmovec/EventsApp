using Entities.Models;
using Web.ViewModels;

namespace Domain.Repositories
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
