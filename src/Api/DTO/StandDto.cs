namespace Api.Dto;

public class StandDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public List<ProductDto> Products { get; set; } = new();
}
