namespace Api.Models;

public sealed class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string ImageUrl { get; set; }
    public required decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public Stand Stand { get; set; } = null!;
    public Guid StandId { get; set; }
}
