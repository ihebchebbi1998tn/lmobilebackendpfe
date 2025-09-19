using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/ServiceRequestAi")]
    [Authorize]
    public class ServiceRequestAiController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            // TODO: Implement get all AI service requests logic
            return Ok(new { totalCount = 0, page, pageSize, data = new object[] { } });
        }

        [HttpPut("payment/{id}/{methode}")]
        public async Task<IActionResult> UpdatePayment(int id, string methode)
        {
            // TODO: Implement payment update logic
            return Ok(new { message = "Payment updated" });
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleServiceRequest([FromForm] object serviceRequest)
        {
            // TODO: Implement toggle logic
            return Ok(new { message = "Service request toggled" });
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id, [FromQuery] string lang, [FromQuery] string city, [FromQuery] string streetName, [FromQuery] string zipCode, [FromQuery] string phone, [FromQuery] string email)
        {
            // TODO: Implement PDF download logic
            return Ok(new { message = "PDF download not implemented" });
        }
    }
}