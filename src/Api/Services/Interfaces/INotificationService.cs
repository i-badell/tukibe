using Api.Dto;

namespace Api.Services.Interfaces;

public interface INotificationService
{
    public Task<int> GetUnreadNotificationsCount(Guid eventId, Guid userId);
}
