using Entities.Models;
using Web.ViewModels;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task BookEventAsync(BookingViewModel viewModel, CancellationToken cancellationToken);
        Task CancelBookingAsync(int id, string userId, CancellationToken cancellationToken);
        Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId, CancellationToken cancellationToken);
        Task<ICollection<UserBrief>> GetEventParticipants(int eventId, CancellationToken cancellationToken);
    }
}
