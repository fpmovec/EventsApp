using Entities.AppSettings;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Web.ViewModels;

namespace Application.Services
{
    public class NotificationService<THub> : INotificationService
        where THub: Hub<INotificationsClient>
    {
        private readonly IHubContext<THub, INotificationsClient> _hubContext;
        private readonly ICacheService _cacheService;
        private readonly AppSettings _appSettings;

        public NotificationService(
            IHubContext<THub, INotificationsClient> hubContext,
            IOptions<AppSettings> appSettings,
            ICacheService cacheService)
        {
            _hubContext = hubContext;
            _appSettings = appSettings.Value;
            _cacheService = cacheService;
        }

        public async Task<ICollection<DetailsChangedEvent>> GetAllNotificationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            ICollection<DetailsChangedEvent> notifications = await _cacheService.GetAsync<DetailsChangedEvent[]>(userId.ToString(), cancellationToken) ?? [];

            return notifications;
        }

        public async Task NotifyCurrentUserWithPopupAsync(string oldName, int eventId, ICollection<UserBrief> users)
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

        public async Task NotifyUsersAsync(string oldName, int eventId, ICollection<UserBrief> users, CancellationToken cancellationToken)
        {
            string notificationMessage = string.Format(_appSettings.EventMessages.ChangedEventInfoMessage, oldName);

            foreach (UserBrief user in users)
            {
                var notifications = await _cacheService.GetAsync<DetailsChangedEvent[]>(user.Id, cancellationToken);

                if (notifications is null)
                    notifications = [];

                List<DetailsChangedEvent> notificationsList = [.. notifications];

                notificationsList.Add(new DetailsChangedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventId = eventId,
                    Message = notificationMessage,
                });

                await _cacheService.SetAsync(user.Id, notificationsList.ToArray(), cancellationToken);
            }
        }
    }
}
