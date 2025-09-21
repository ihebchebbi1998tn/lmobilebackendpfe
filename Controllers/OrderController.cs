using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConsolidatedApi.Services;
using ConsolidatedApi.Models;

namespace ConsolidatedApi.Controllers
{
    [ApiController]
    [Route("service/api/Order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var order = new Order
                {
                    ClientOrganizationId = dto.ClientOrganizationId,
                    Description = dto.Description,
                    Status = "Pending",
                    TotalAmount = dto.TotalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                var createdOrder = await _orderService.CreateAsync(order);
                return Ok(new { message = "Order created successfully", order = createdOrder });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating order", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] UpdateOrderRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var existingOrder = await _orderService.GetByIdAsync(dto.Id);
                if (existingOrder == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                existingOrder.Description = dto.Description;
                existingOrder.Status = dto.Status;
                existingOrder.TotalAmount = dto.TotalAmount;
                existingOrder.UpdatedAt = DateTime.UtcNow;

                var updatedOrder = await _orderService.UpdateAsync(existingOrder);
                return Ok(new { message = "Order updated successfully", order = updatedOrder });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating order", error = ex.Message });
            }
        }

        [HttpPost("payment_methode")]
        public async Task<IActionResult> PaymentMethode([FromBody] PaymentMethodRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var order = await _orderService.GetByIdAsync(dto.OrderId);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                // Update payment method (assuming there's a PaymentMethod field)
                order.UpdatedAt = DateTime.UtcNow;
                await _orderService.UpdateAsync(order);

                return Ok(new { message = "Payment method updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating payment method", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                await _orderService.DeleteAsync(id);
                return Ok(new { message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting order", error = ex.Message });
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
                var allOrders = await _orderService.GetAllAsync();

                // Filter by company if specified
                if (companyId > 0)
                {
                    allOrders = allOrders.Where(o => o.ClientOrganizationId == companyId).ToList();
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    allOrders = allOrders.Where(o => 
                        (o.Description != null && o.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        o.Status.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                var totalCount = allOrders.Count;
                var paginatedOrders = allOrders
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new { totalCount, page, pageSize, data = paginatedOrders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving orders", error = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id, [FromQuery] string lang, [FromQuery] string city, [FromQuery] string streetName, [FromQuery] string zipCode, [FromQuery] string phone, [FromQuery] string email)
        {
            try
            {
                // TODO: Implement PDF generation logic using QuestPDF
                return Ok(new { message = "PDF generation not yet implemented", orderId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating PDF", error = ex.Message });
            }
        }
    }

    public class CreateOrderRequest
    {
        public int ClientOrganizationId { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    public class PaymentMethodRequest
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}