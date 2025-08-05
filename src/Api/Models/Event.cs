namespace Api.Models;

public class Event
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<Stand> Stands { get; set; } = new List<Stand>();
}
