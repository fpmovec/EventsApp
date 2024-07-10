using Domain.Models;
using Web.ViewModels;

namespace Application.Services
{
    public interface INotificationService
    {
        Task NotifyUsersAsync(EventBaseModel eventBaseModel, ICollection<UserBrief> users);

        Task NotifyCurrentUserWithPopupAsync(EventBaseModel eventBaseModel, ICollection<UserBrief> users);
    }
}
