using Api.Dto;

namespace Api.Services;

public interface IEventService
{
    public Task<StandResponse?> GetStandData(Guid eventId, Guid standId);
    public Task<EventStandsResponse?> GetEventStands(Guid eventId);
}
