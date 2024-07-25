using Domain.Models;
using Web.DTO;

namespace Application.Services
{
    public interface INotificationService
    {
        Task NotifyUsersAsync(string oldName, int eventId, ICollection<UserBrief> users, CancellationToken cancellationToken);

        Task NotifyCurrentUserWithPopupAsync(string oldName, int eventId, ICollection<UserBrief> users);

        Task<ICollection<DetailsChangedEvent>> GetAllNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
