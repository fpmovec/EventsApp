using Domain.Models;
using Web.DTO;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task BookEventAsync(BookingDTO viewModel, CancellationToken cancellationToken);
        Task CancelBookingAsync(int id, string userId, CancellationToken cancellationToken);
        Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId, CancellationToken cancellationToken);
        Task<ICollection<UserBrief>> GetEventParticipants(int eventId, CancellationToken cancellationToken);
    }
}
