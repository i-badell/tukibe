using Api.Dto;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        var count = await _notificationDataService.GetUnreadNotificationsCount(e.Id, user.Id);
        
        return Ok(new NotificationsCountResponse { UnreadCount = count });
    }

    [Authorize]
    [HttpGet("{eventId:guid}/notifications")]
    [ProducesResponseType<List<NotificationDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotifications(Guid eventId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        var data = await _notificationDataService.GetNotifications(e.Id, user.Id);

        return Ok(data);
    }

    [Authorize]
    [HttpDelete("{eventId:guid}/notifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllNotifications(Guid eventId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.DeleteAllNotifications(e.Id, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros"});
        }
    }

    [Authorize]
    [HttpDelete("{eventId:guid}/notifications/{notificationId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteNotificationById(Guid eventId, Guid notificationId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.DeleteNotificationById(notificationId, eventId, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros" });
        }
    }

    [Authorize]
    [HttpPut("{eventId:guid}/notifications/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReadAllNotifications(Guid eventId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.ReadAllNotifications(e.Id, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros" });
        }
    }

    [Authorize]
    [HttpPut("{eventId:guid}/notifications/read/{notificationId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReadNotificationById(Guid eventId, Guid notificationId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.ReadNotificationById(notificationId, eventId, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros" });
        }
    }
    [Authorize]
    [HttpPut("{eventId:guid}/notifications/unread")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnreadAllNotifications(Guid eventId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.UnreadAllNotifications(e.Id, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros" });
        }
    }

    [Authorize]
    [HttpPut("{eventId:guid}/notifications/unread/{notificationId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnreadNotificationById(Guid eventId, Guid notificationId)
    {
        var e = await _eventDataService.GetEventById(eventId);
        if (e is null)
            return NotFound(new { ErrorMsg = "No se encontró el evento" });

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUserByAuth0Id(auth0Id);
        if (user is null)
            return NotFound(new { ErrorMsg = "No se encontró el usuario" });

        try
        {
            await _notificationDataService.UnreadNotificationById(notificationId, eventId, user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(500, new { ErrorMsg = "Ocurrió un error al actualizar los registros" });
        }
    }
}
