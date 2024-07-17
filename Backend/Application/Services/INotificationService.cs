using Domain.Models;
using Web.ViewModels;

namespace Application.Services
{
    public interface INotificationService
    {
        Task NotifyUsersAsync(string oldName, int eventId, ICollection<UserBrief> users);

        Task NotifyCurrentUserWithPopupAsync(string oldName, int eventId, ICollection<UserBrief> users);

        Task<ICollection<DetailsChangedEvent>> GetAllNotificationsAsync(Guid userId);
    }
}
