namespace Api.Dto;

public class CatalogDto
{
    public Guid CatalogId { get; set; }
    public List<ProductDto> Products { get; set; } = new();
}
