using Api.Context;
using Api.Dto;
using Api.Helpers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/events/")]
public class EventController : ControllerBase
{
    private readonly ApiContext _apiContext;
    private readonly ILogger<EventController> _logger;

    public EventController(ApiContext apiContext, ILogger<EventController> logger)
    {
        _apiContext = apiContext;
        _logger = logger;
    }

    [HttpGet("{eventId:guid}/stands/{standId:guid}")]
    [ProducesResponseType<StandResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStand(Guid eventId, Guid standId, [FromQuery] bool includeProducts = false)
    {
        var query = _apiContext.Stands;
        if (includeProducts)
        {
            query.Include(x => x.Products);
        }
        var stand = await query.FindAsync(eventId, standId);
        if (stand is null)
        {
            return NotFound();
        }

        return Ok(stand.ToDto());
    }

    [HttpGet("{eventId:guid}/stands")]
    [ProducesResponseType<EventStandsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventStands(Guid eventId, [FromQuery] int numberOfProducts = 5)
    {
        var stands = await _apiContext.Stands
            .Where(x => x.EventId == eventId)
            .Include(x => x.Products
                .Where(x => x.IsAvailable)
                .OrderByDescending(x => x.Price)
                .Take(numberOfProducts))
            .ToListAsync();

        if (stands == null)
        {
            return NotFound();
        }
        return Ok(stands.ToDto());
    }
}
