using Api.Context;
using Api.Dto;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/stand/{standId:guid}/cart")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly ApiContext _context;

    public CartController(ILogger<CartController> logger, ApiContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType<Cart>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(Guid standId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.StandId == standId);
        if (cart is null)
        {
            return NotFound(new { ErrorMsg = "No existe carrito para este stand." });
        }
        //TODO: Map to DTO
        return Ok(cart);
    }

    [HttpPut]
    [ProducesResponseType<Cart>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpsertCart(Guid standId, [FromBody] Cart cart)
    {
        var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.StandId == standId);
        if (existingCart is null || existingCart.IsCompleted == true)
        {
            if (existingCart is not null)
            {
                _context.Carts.Remove(existingCart!);
                await _context.SaveChangesAsync();
            }
            cart.Id = Guid.NewGuid();
            cart.StandId = standId;
            _context.Carts.Add(cart);
            return CreatedAtAction(nameof(GetCart), new { standId = standId }, cart);
        }

        existingCart.Items = cart.Items;
        _context.Carts.Update(existingCart);
        await _context.SaveChangesAsync();
        return Ok(cart);
    }


}
