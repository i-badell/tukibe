using Api.Dto;

namespace Api.Services;

public interface IEventService
{
    public Task<EventProducts?> GetEventProducts(Guid eventId);
}
