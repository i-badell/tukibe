namespace Api.Models;

public class Catalog
{
    public Guid Id { get; set; }

    public Guid? StandId { get; set; }
    public Stand? Stand { get; set; }

    public Guid? EventId { get; set; }
    public Event? Event { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

