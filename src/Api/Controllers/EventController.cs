using Api.Services;
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

    [HttpGet("{id:guid}/products")]
    public async Task<IActionResult> GetFullEventData(Guid id)
    {
        var data = await _eventDataService.GetEventProducts(id);
        if (data == null)
            return NotFound();
        return Ok(data);
    }
}
