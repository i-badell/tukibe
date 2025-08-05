namespace Api.Models;

public class Stand 
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public Guid EventId { get; set; }
    public required Event Event { get; set; }
    public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
}
