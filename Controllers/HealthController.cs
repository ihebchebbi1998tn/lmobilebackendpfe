using Microsoft.AspNetCore.Mvc;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                service = "ConsolidatedApi"
            });
        }
    }
}