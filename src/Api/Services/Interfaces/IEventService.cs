using Api.Dto;

namespace Api.Services.Interfaces;

public interface IEventService
{
    public Task<StandResponse?> GetStandData(Guid eventId, Guid standId);
    public Task<EventStandsResponse?> GetEventStands(Guid eventId);
    public Task<NotificationsCountResponse?> GetUserNotificationsCount(Guid eventId, string auth0Id);    
}
