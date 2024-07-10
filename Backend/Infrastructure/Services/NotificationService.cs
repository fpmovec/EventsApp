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
        private readonly IAuthService _authService;

        public NotificationService(
            IHubContext<NotificationsHub, INotificationsClient> hubContext,
            IOptions<AppSettings> appSettings,
            ICacheService cacheService,
            IAuthService authService)
        {
            _hubContext = hubContext;
            _appSettings = appSettings.Value;
            _cacheService = cacheService;
            _authService = authService;
        }

        public async Task NotifyCurrentUserWithPopupAsync(EventBaseModel eventBaseModel, ICollection<UserBrief> users)
        {
            string notificationMessage = string.Format(_appSettings.EventMessages.ChangedEventInfoMessage, eventBaseModel.Name);
            var currentUser = await _authService.GetCurrentUserAsync();
            await _hubContext.Clients.All.ReceiveMessageAsync(new DetailsChangedEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventId = eventBaseModel.Id,
                Message = notificationMessage,
                UserIds = users.Select(x => x.Id).ToArray(),
            });
        }

        public async Task NotifyUsersAsync(EventBaseModel eventBaseModel, ICollection<UserBrief> users)
        {
            string notificationMessage = string.Format(_appSettings.EventMessages.ChangedEventInfoMessage, eventBaseModel.Name);

            foreach (UserBrief user in users)
            {
                var notifications = await _cacheService.GetAsync<DetailsChangedEvent[]>(user.Id);

                if (notifications is null)
                    notifications = [];

                List<DetailsChangedEvent> notificationsList = notifications.ToList();

                notificationsList.Add(new DetailsChangedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventId = eventBaseModel.Id,
                    Message = notificationMessage,
                });

                await _cacheService.SetAsync(user.Id, notificationsList.ToArray());
            }
        }
    }
}
