using Api.Context;
using Api.Dto;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Api.Services;

public class NotificationService : INotificationService
{
    private readonly ClientContext _context;

    public NotificationService(ClientContext context)
    {
        _context = context;
    }
    
    public async Task<int> GetUnreadNotificationsCount(Guid eventId, Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.EventId == eventId)
            .Where(n => n.UserId == userId)
            .Where(n => !n.IsRead && !n.IsDeleted)
            .CountAsync();
    }
    public async Task<List<NotificationDto>> GetNotifications(Guid eventId, Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.EventId == eventId)
            .Where(n => n.UserId == userId)
            .Where(n => !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                NotificationId = n.Id,
                NotificationTitle = n.Title,
                NotificationMessage = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
            })
            .ToListAsync();
            
    }
}
