using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConsolidatedApi.Services;
using ConsolidatedApi.Models;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/CustomerDevices")]
    [Authorize]
    public class CustomerDevicesController : ControllerBase
    {
        private readonly CustomerDevicesService _customerDevicesService;

        public CustomerDevicesController(CustomerDevicesService customerDevicesService)
        {
            _customerDevicesService = customerDevicesService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customerDevice = await _customerDevicesService.GetByIdAsync(id);
                if (customerDevice == null)
                {
                    return NotFound(new { message = "Customer device not found" });
                }
                return Ok(customerDevice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving customer device", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCustomerDeviceRequest customerDevice)
        {
            try
            {
                var device = new CustomerDevice
                {
                    DeviceId = customerDevice.DeviceId,
                    CustomerId = customerDevice.CustomerId,
                    PurchaseDate = customerDevice.PurchaseDate,
                    WarrantyExpiry = customerDevice.WarrantyExpiry,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdDevice = await _customerDevicesService.CreateAsync(device);
                return Ok(new { message = "Customer device created successfully", customerDevice = createdDevice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating customer device", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customerDevice = await _customerDevicesService.GetByIdAsync(id);
                if (customerDevice == null)
                {
                    return NotFound(new { message = "Customer device not found" });
                }

                await _customerDevicesService.DeleteAsync(id);
                return Ok(new { message = "Customer device deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting customer device", error = ex.Message });
            }
        }
    }

    public class CreateCustomerDeviceRequest
    {
        public int DeviceId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
    }
}
}