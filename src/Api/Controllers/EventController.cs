using Api.Dto;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventDataService;

    public EventController(IEventService eventDataService)
    {
        _eventDataService = eventDataService;
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
}
