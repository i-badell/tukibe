using Api.Dto;

namespace Api.Services.Interfaces;

public interface INotificationService
{
    public Task<int> GetUnreadNotificationsCount(Guid eventId, Guid userId);
    public Task<List<NotificationDto>> GetNotifications(Guid eventId, Guid userId);
    public Task DeleteAllNotifications(Guid eventId, Guid userId);
    public Task DeleteNotificationById(Guid notificationId, Guid eventId, Guid userId);
    public Task ReadAllNotifications(Guid eventId, Guid userId);
    public Task ReadNotificationById(Guid notificationId, Guid eventId, Guid userId);
    public Task UnreadAllNotifications(Guid eventId, Guid userId);
    public Task UnreadNotificationById(Guid notificationId, Guid eventId, Guid userId);
}
