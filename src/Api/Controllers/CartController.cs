using Api.Dto;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/cart")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;

    public CartController(IEventService eventDataService, ILogger<CartController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType<StandResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullStandData(Guid eventId, Guid standId)
    {
        throw new NotImplementedException();
    }

}
