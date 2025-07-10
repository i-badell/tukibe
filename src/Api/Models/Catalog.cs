namespace Api.Models;

public class Catalog
{
    public Guid Id { get; set; }

    public Guid? StandId { get; set; }
    public Stand? Stand { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}

