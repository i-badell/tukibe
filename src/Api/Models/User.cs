namespace Api.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Auth0Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
