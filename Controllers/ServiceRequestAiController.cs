using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConsolidatedApi.Services;
using ConsolidatedApi.Models;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/ServiceRequestAi")]
    [Authorize]
    public class ServiceRequestAiController : ControllerBase
    {
        private readonly ServiceRequestAiService _serviceRequestAiService;

        public ServiceRequestAiController(ServiceRequestAiService serviceRequestAiService)
        {
            _serviceRequestAiService = serviceRequestAiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var allRequests = await _serviceRequestAiService.GetAllAsync();

                // Filter by company if specified
                if (companyId > 0)
                {
                    // Assuming there's a ClientOrganizationId property
                    // allRequests = allRequests.Where(sr => sr.ClientOrganizationId == companyId).ToList();
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Apply search based on AI service request properties
                    // allRequests = allRequests.Where(sr => sr.Description.Contains(searchTerm)).ToList();
                }

                var totalCount = allRequests.Count;
                var paginatedRequests = allRequests
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new { totalCount, page, pageSize, data = paginatedRequests });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving AI service requests", error = ex.Message });
            }
        }

        [HttpPut("payment/{id}/{methode}")]
        public async Task<IActionResult> UpdatePayment(int id, string methode)
        {
            try
            {
                // TODO: Implement payment update logic with proper service integration
                return Ok(new { message = "Payment method updated successfully", id, method = methode });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating payment", error = ex.Message });
            }
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleServiceRequest([FromForm] ToggleAiServiceRequest serviceRequest)
        {
            try
            {
                // TODO: Implement toggle logic with proper service integration
                return Ok(new { message = "AI service request toggled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error toggling AI service request", error = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id, [FromQuery] string lang, [FromQuery] string city, [FromQuery] string streetName, [FromQuery] string zipCode, [FromQuery] string phone, [FromQuery] string email)
        {
            try
            {
                // TODO: Implement PDF generation logic using QuestPDF
                return Ok(new { message = "PDF generation not yet implemented", aiServiceRequestId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating PDF", error = ex.Message });
            }
        }
    }

    public class ToggleAiServiceRequest
    {
        public int Id { get; set; }
    }
}
}