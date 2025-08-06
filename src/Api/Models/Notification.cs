namespace Api.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public required Event Event { get; set; }
    public Guid UserId { get; set; }
    public required User User { get; set; }
    public required string Title { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead  { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
}
