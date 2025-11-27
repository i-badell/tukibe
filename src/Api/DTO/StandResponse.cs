using Api.Models;

namespace Api.Dto;

public class StandResponse
{
	public Guid EventId { get; set; }
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public string? ImageUrl { get; set; }
	public List<ProductDto> Products { get; set; } = new();
}
