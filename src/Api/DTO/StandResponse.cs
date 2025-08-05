using Api.Models;

namespace Api.Dto;

public class StandResponse
{
    public Guid EventId { get; set; }
    public Guid StandId { get; set; }
    public string? StandName { get; set; }
    public List<ProductDto> Products { get; set; } = new();
}
