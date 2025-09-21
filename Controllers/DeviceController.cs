using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConsolidatedApi.Services;
using ConsolidatedApi.Models;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/Devices")]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService _deviceService;

        public DeviceController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateDeviceRequest dto)
        {
            try
            {
                var device = new Device
                {
                    Name = dto.Name,
                    Model = dto.Model,
                    SerialNumber = dto.SerialNumber,
                    Description = dto.Description,
                    ClientOrganizationId = dto.ClientOrganizationId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdDevice = await _deviceService.CreateAsync(device);
                return Ok(new { message = "Device created successfully", device = createdDevice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating device", error = ex.Message });
            }
        }

        [HttpPost("add_list_of_devices")]
        public async Task<IActionResult> AddListDevices([FromForm] List<CreateDeviceRequest> devices)
        {
            try
            {
                var createdDevices = new List<Device>();
                foreach (var deviceDto in devices)
                {
                    var device = new Device
                    {
                        Name = deviceDto.Name,
                        Model = deviceDto.Model,
                        SerialNumber = deviceDto.SerialNumber,
                        Description = deviceDto.Description,
                        ClientOrganizationId = deviceDto.ClientOrganizationId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var createdDevice = await _deviceService.CreateAsync(device);
                    createdDevices.Add(createdDevice);
                }

                return Ok(new { message = "Devices added successfully", devices = createdDevices });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error adding devices", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] UpdateDeviceRequest device)
        {
            try
            {
                var existingDevice = await _deviceService.GetByIdAsync(device.Id);
                if (existingDevice == null)
                {
                    return NotFound(new { message = "Device not found" });
                }

                existingDevice.Name = device.Name;
                existingDevice.Model = device.Model;
                existingDevice.SerialNumber = device.SerialNumber;
                existingDevice.Description = device.Description;
                existingDevice.UpdatedAt = DateTime.UtcNow;

                var updatedDevice = await _deviceService.UpdateAsync(existingDevice);
                return Ok(new { message = "Device updated successfully", device = updatedDevice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating device", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allDevices = await _deviceService.GetAllAsync();

                // Filter by company if specified
                if (companyId > 0)
                {
                    allDevices = allDevices.Where(d => d.ClientOrganizationId == companyId).ToList();
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    allDevices = allDevices.Where(d => 
                        d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        d.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (d.SerialNumber != null && d.SerialNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                var totalCount = allDevices.Count;
                var paginatedDevices = allDevices
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new { totalCount, page, pageSize, data = paginatedDevices });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving devices", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var device = await _deviceService.GetByIdAsync(id);
                if (device == null)
                {
                    return NotFound(new { message = "Device not found" });
                }

                await _deviceService.DeleteAsync(id);
                return Ok(new { message = "Device deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting device", error = ex.Message });
            }
        }
    }

    public class CreateDeviceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? SerialNumber { get; set; }
        public string? Description { get; set; }
        public int ClientOrganizationId { get; set; }
    }

    public class UpdateDeviceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? SerialNumber { get; set; }
        public string? Description { get; set; }
    }
}