using Api.Dto;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationDataService;
    private readonly IEventService _eventDataService;
    private readonly IUserService _userService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(IEventService eventDataService, 
            INotificationService notificationDataService, 
            IUserService userDataService, 
            ILogger<NotificationController> logger)
    {
        _notificationDataService = notificationDataService;
        _eventDataService = eventDataService;
        _userService = userDataService;
        _logger = logger;
    }
    
    [Authorize]
    [HttpGet("{eventId:guid}/notifications/count")]
    [ProducesResponseType<NotificationsCountResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnreadNotificationsCount(Guid eventId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound();

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound();

        var count = await _notificationDataService.GetUnreadNotificationsCount(e.Id, user.Id);
        
        return Ok(new NotificationsCountResponse { UnreadCount = count });
    }
}
