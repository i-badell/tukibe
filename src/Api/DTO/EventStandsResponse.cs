using Api.Models;

namespace Api.Dto;

public class EventStandsResponse
{
    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public string? EventImageUrl { get; set; }
    public StandDto? Stand { get; set; }
    public List<StandDto> Stands { get; set; } = new();
}
