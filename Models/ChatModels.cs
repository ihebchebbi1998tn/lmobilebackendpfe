namespace ConsolidatedApi.Models;

public class ChatSession
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public List<ChatMessage> Messages { get; set; } = new();
}

public class ChatMessage
{
    public int Id { get; set; }
    public int ChatSessionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "user" or "assistant"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ChatSession ChatSession { get; set; } = null!;
}

public class UiPage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Route { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class GeminiPrompt
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}