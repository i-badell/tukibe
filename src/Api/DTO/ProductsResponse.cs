namespace Api.Dto;

public class ProductResponse
{
    public Guid EventId { get; set; }
    public List<ProductDto> Catalog { get; set; } = new();
    public List<StandDto> Stands { get; set; } = new();
}
