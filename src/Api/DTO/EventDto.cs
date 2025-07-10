namespace Api.Dto;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool InStock { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = default!;
}
