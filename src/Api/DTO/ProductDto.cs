namespace Api.Dto;

public class ProductDto
{
	public Guid Id { get; set; }
	public Guid StandId { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public decimal Price { get; set; }
	public string ImageUrl { get; set; } = default!;
}
