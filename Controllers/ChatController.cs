using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("chat/api/chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        [HttpGet("sessions")]
        public async Task<IActionResult> GetSessions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement actual chat session logic
            // For now, return empty array to prevent 404
            return Ok(new[] { });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement delete session logic
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTitle([FromBody] ChatUpdateDto dto, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // TODO: Implement update title logic
            return Ok();
        }
    }

    public class ChatUpdateDto
    {
        public string Title { get; set; } = string.Empty;
    }
}