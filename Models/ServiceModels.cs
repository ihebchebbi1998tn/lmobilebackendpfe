namespace ConsolidatedApi.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string Status { get; set; } = "Active"; // Active, Inactive, Maintenance
    public int? ClientOrganizationId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ClientOrganization? ClientOrganization { get; set; }
}

public class CustomerDevice
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public DateTime InstallationDate { get; set; }
    public string Status { get; set; } = "Active"; // Active, Inactive, Maintenance
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Device Device { get; set; } = null!;
}

public class SparePart
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PartNumber { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int QuantityInStock { get; set; }
    public int MinimumStock { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class ServiceRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
    public int? DeviceId { get; set; }
    public string? AssignedUserId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Device? Device { get; set; }
    public ApplicationUser? AssignedUser { get; set; }
}

public class InstallationRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    public int? DeviceId { get; set; }
    public string? AssignedUserId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? InstallationAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Device? Device { get; set; }
    public ApplicationUser? AssignedUser { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
    public decimal TotalAmount { get; set; }
    public string? UserId { get; set; }
    public string? ShippingAddress { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser? User { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public decimal Amount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue, Cancelled
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Order? Order { get; set; }
}

public class Feedback
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // General, Bug, Feature, Support
    public int Rating { get; set; } = 5; // 1-5 stars
    public string Status { get; set; } = "Open"; // Open, InReview, Closed
    public string? UserId { get; set; }
    public string? Response { get; set; }
    public DateTime? ResponseDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser? User { get; set; }
}

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = "Info"; // Info, Warning, Error, Success
    public bool IsRead { get; set; } = false;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
}