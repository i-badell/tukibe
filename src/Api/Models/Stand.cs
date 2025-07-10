namespace Api.Models;

public class Stand
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
