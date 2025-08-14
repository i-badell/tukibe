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
}
