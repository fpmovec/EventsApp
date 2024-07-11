using Application;
using Application.Services;
using Domain.AppSettings;
using Domain.Models;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Web.ViewModels;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> _hubContext;
        private readonly ICacheService _cacheService;
        private readonly AppSettings _appSettings;

        public NotificationService(
            IHubContext<NotificationsHub, INotificationsClient> hubContext,
            IOptions<AppSettings> appSettings,
            ICacheService cacheService)
        {
            _hubContext = hubContext;
            _appSettings = appSettings.Value;
            _cacheService = cacheService;
        }

        public async Task<ICollection<DetailsChangedEvent>> GetAllNotificationsAsync(string userId)
        {
            ICollection<DetailsChangedEvent> notifications = await _cacheService.GetAsync<DetailsChangedEvent[]>(userId) ?? [];

            return notifications;
        }

        public async Task NotifyCurrentUserWithPopupAsync(string oldName, Guid eventId, ICollection<UserBrief> users)
        {
            string notificationMessage = string.Format(_appSettings.EventMessages.ChangedEventInfoMessage, oldName);

            await _hubContext.Clients.All.ReceiveMessageAsync(new DetailsChangedEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventId = eventId,
                Message = notificationMessage,
                UserIds = users.Select(x => x.Id).ToArray(),
            });
        }

        public async Task NotifyUsersAsync(string oldName, Guid eventId, ICollection<UserBrief> users)
        {
            string notificationMessage = string.Format(_appSettings.EventMessages.ChangedEventInfoMessage, oldName);

            foreach (UserBrief user in users)
            {
                var notifications = await _cacheService.GetAsync<DetailsChangedEvent[]>(user.Id);

                if (notifications is null)
                    notifications = [];

                List<DetailsChangedEvent> notificationsList = [..notifications];

                notificationsList.Add(new DetailsChangedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventId = eventId,
                    Message = notificationMessage,
                });

                await _cacheService.SetAsync(user.Id, notificationsList.ToArray());
            }
        }
    }
}
