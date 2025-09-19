using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/InstallationRequest")]
    [Authorize]
    public class InstallationRequestController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: Implement get installation request by id logic
            return Ok(new { message = "Installation request found" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] object installationRequest)
        {
            // TODO: Implement installation request creation logic
            return Ok(new { message = "Installation request created" });
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromForm] object installationRequest)
        {
            // TODO: Implement toggle logic
            return Ok(new { message = "Installation request toggled" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Implement installation request delete logic
            return Ok(new { message = "Installation request deleted" });
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id, [FromQuery] string lang, [FromQuery] string city, [FromQuery] string streetName, [FromQuery] string zipCode, [FromQuery] string phone, [FromQuery] string email)
        {
            // TODO: Implement PDF download logic
            return Ok(new { message = "PDF download not implemented" });
        }
    }
}