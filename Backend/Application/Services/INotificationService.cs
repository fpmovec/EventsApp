using Domain.Models;
using Web.ViewModels;

namespace Application.Services
{
    public interface INotificationService
    {
        Task NotifyUsersAsync(string oldName, Guid eventId, ICollection<UserBrief> users);

        Task NotifyCurrentUserWithPopupAsync(string oldName, Guid eventId, ICollection<UserBrief> users);

        Task<ICollection<DetailsChangedEvent>> GetAllNotificationsAsync(string userId);
    }
}
