using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConsolidatedApi.Services;
using ConsolidatedApi.Models;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/ServiceRequest")]
    [Authorize]
    public class ServiceRequestController : ControllerBase
    {
        private readonly ServiceRequestService _serviceRequestService;

        public ServiceRequestController(ServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateServiceRequestRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var serviceRequest = new ServiceRequest
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Status = "Pending",
                    Priority = dto.Priority,
                    ClientOrganizationId = dto.ClientOrganizationId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    RequestedBy = userId
                };

                var createdServiceRequest = await _serviceRequestService.CreateAsync(serviceRequest);
                return Ok(new { message = "Service request created successfully", serviceRequest = createdServiceRequest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating service request", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] UpdateServiceRequestRequest dto)
        {
            try
            {
                var existingServiceRequest = await _serviceRequestService.GetByIdAsync(dto.Id);
                if (existingServiceRequest == null)
                {
                    return NotFound(new { message = "Service request not found" });
                }

                existingServiceRequest.Title = dto.Title;
                existingServiceRequest.Description = dto.Description;
                existingServiceRequest.Status = dto.Status;
                existingServiceRequest.Priority = dto.Priority;
                existingServiceRequest.UpdatedAt = DateTime.UtcNow;

                var updatedServiceRequest = await _serviceRequestService.UpdateAsync(existingServiceRequest);
                return Ok(new { message = "Service request updated successfully", serviceRequest = updatedServiceRequest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating service request", error = ex.Message });
            }
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromForm] ToggleServiceRequestRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var serviceRequest = await _serviceRequestService.GetByIdAsync(dto.Id);
                if (serviceRequest == null)
                {
                    return NotFound(new { message = "Service request not found" });
                }

                // Toggle status (e.g., Active/Inactive or Pending/Completed)
                serviceRequest.Status = serviceRequest.Status == "Active" ? "Inactive" : "Active";
                serviceRequest.UpdatedAt = DateTime.UtcNow;

                await _serviceRequestService.UpdateAsync(serviceRequest);
                return Ok(new { message = "Service request status toggled successfully", status = serviceRequest.Status });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error toggling service request status", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var serviceRequest = await _serviceRequestService.GetByIdAsync(id);
                if (serviceRequest == null)
                {
                    return NotFound(new { message = "Service request not found" });
                }

                await _serviceRequestService.DeleteAsync(id);
                return Ok(new { message = "Service request deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting service request", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var allServiceRequests = await _serviceRequestService.GetAllAsync();

                // Filter by company if specified
                if (companyId > 0)
                {
                    allServiceRequests = allServiceRequests.Where(sr => sr.ClientOrganizationId == companyId).ToList();
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    allServiceRequests = allServiceRequests.Where(sr => 
                        sr.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (sr.Description != null && sr.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        sr.Status.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                var totalCount = allServiceRequests.Count;
                var paginatedServiceRequests = allServiceRequests
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new { totalCount, page, pageSize, data = paginatedServiceRequests });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving service requests", error = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id, [FromQuery] string lang, [FromQuery] string city, [FromQuery] string streetName, [FromQuery] string zipCode, [FromQuery] string phone, [FromQuery] string email)
        {
            try
            {
                // TODO: Implement PDF generation logic using QuestPDF
                return Ok(new { message = "PDF generation not yet implemented", serviceRequestId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating PDF", error = ex.Message });
            }
        }
    }

    public class CreateServiceRequestRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Medium";
        public int ClientOrganizationId { get; set; }
    }

    public class UpdateServiceRequestRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
    }

    public class ToggleServiceRequestRequest
    {
        public int Id { get; set; }
    }
}