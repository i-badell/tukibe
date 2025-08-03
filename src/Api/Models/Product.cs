namespace Api.Models;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public required Category Category { get; set; }
    public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
}
