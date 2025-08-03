using Api.Dto;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    //[Authorize]
    [HttpGet("{eventId:guid}/products")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullEventData(Guid eventId)
    {
        var data = await _eventDataService.GetEventProducts(eventId);
        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }
}
