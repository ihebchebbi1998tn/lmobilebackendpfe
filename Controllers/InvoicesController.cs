using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/Invoices")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            // TODO: Implement get all invoices logic
            return Ok(new object[] { });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object invoice)
        {
            // TODO: Implement invoice creation logic
            return Ok(new { message = "Invoice created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object invoice)
        {
            // TODO: Implement invoice update logic
            return Ok(new { message = "Invoice updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Implement invoice delete logic
            return Ok(new { message = "Invoice deleted" });
        }
    }
}