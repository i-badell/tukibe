using Api.Dto;

namespace Api.Services;

public interface IEventService
{
    public Task<ProductResponse?> GetEventProducts(Guid eventId);
}
