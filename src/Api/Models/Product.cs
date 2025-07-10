namespace Api.Models;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool InStock { get; set; }
    public decimal Price { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
}
