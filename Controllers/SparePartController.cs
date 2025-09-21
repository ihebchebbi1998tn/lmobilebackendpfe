using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/SparePart")]
    [Authorize]
    public class SparePartController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] object dto)
        {
            // TODO: Implement spare part creation logic
            return Ok(new { message = "Spare part created" });
        }

        [HttpPost("add_list_of_spareParts")]
        public async Task<IActionResult> AddListSpareParts([FromForm] object data)
        {
            // TODO: Implement add list of spare parts logic
            return Ok(new { message = "Spare parts added" });
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] object sparePart)
        {
            // TODO: Implement spare part update logic
            return Ok(new { message = "Spare part updated successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // TODO: Implement get all spare parts logic
            return Ok(new { totalCount = 0, page, pageSize, data = new object[] { } });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Implement spare part delete logic
            return Ok(new { message = "Spare part deleted successfully" });
        }
    }
}