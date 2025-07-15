namespace Api.Models;

public class Event
{
    public Guid Id { get; set; }
    public Catalog? Catalog { get; set; }
    public ICollection<Stand> Stands { get; set; } = new List<Stand>();
}
