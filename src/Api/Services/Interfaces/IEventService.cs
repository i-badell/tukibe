using Api.Dto;
using Api.Models;

namespace Api.Services.Interfaces;

public interface IEventService
{
    public Task<StandResponse?> GetStandData(Guid eventId, Guid standId);
    public Task<EventStandsResponse?> GetEventStands(Guid eventId);  
    public Task<Event?> GetEventById(Guid eventId);
}
