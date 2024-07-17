using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetAllNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationService.GetAllNotificationsAsync(userId);

            return Ok(notifications);
        }
    }
}
