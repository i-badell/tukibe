namespace Api.Dto;

public class ProductResponse
{
    public Guid EventId { get; set; }
    public List<CatalogDto> Catalogs { get; set; } = new();
    public List<StandDto> Stands { get; set; } = new();
}
