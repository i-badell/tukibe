namespace Api.Models;

public class Event
{
    public Guid Id { get; set; }
    public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
    public ICollection<Stand> Stands { get; set; } = new List<Stand>();
}
