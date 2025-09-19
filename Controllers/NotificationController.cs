using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("notification/api/notification")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement actual notification logic
            // For now, return empty array to prevent 404
            return Ok(new[] { });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement delete notification logic
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement mark as read logic
            return Ok();
        }
    }
}