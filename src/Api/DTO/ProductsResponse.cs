namespace Api.Dto;

public class ProductResponse
{
    public Guid EventId { get; set; }
    public CatalogDto? Catalogs { get; set; }
    public List<StandDto> Stands { get; set; } = new();
}
