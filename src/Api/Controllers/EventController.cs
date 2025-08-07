using Api.Dto;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventDataService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventDataService, ILogger<EventController> logger)
    {
        _eventDataService = eventDataService;
        _logger = logger;
    }

    [HttpGet("{eventId:guid}/stands/{standId:guid}")]
    [ProducesResponseType<StandResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullStandData(Guid eventId, Guid standId)
    {
        var data = await _eventDataService.GetStandData(eventId, standId);
        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }

    [HttpGet("{eventId:guid}/stands")]
    [ProducesResponseType<EventStandsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventStands(Guid eventId)
    {
        var data = await _eventDataService.GetEventStands(eventId);
        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }

    [Authorize]
    [HttpGet("{eventId:guid}/notifications/count")]
    [ProducesResponseType<NotificationsCountResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserNotificationsCount(Guid eventId)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //var auth0Id = "auth0|demo-user";
        foreach (var claim in User.Claims)
        {
            _logger.LogInformation($"{claim.Type}: {claim.Value}");
        }
        _logger.LogInformation($"auth0id: {auth0Id}");

        
        var data = await _eventDataService.GetUserNotificationsCount(eventId, auth0Id);
        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }
}
