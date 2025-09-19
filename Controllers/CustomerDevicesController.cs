using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/CustomerDevices")]
    [Authorize]
    public class CustomerDevicesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: Implement get customer device by id logic
            return Ok(new { message = "Customer device found" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] object customerDevice)
        {
            // TODO: Implement customer device creation logic
            return Ok(new { message = "Customer device created" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Implement customer device delete logic
            return Ok(new { message = "Customer device deleted" });
        }
    }
}