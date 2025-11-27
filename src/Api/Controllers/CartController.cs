using Api.Context;
using Api.Dto;
using Api.Helpers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/stands/{standId:guid}/cart")]
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
	[ProducesResponseType<CartDto>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CartDto>> GetCart(Guid eventId, Guid standId)
	{
		var standExists = await _context.Stands.AnyAsync(s =>
			s.Id == standId && s.EventId == eventId
		);
		if (!standExists)
		{
			return NotFound();
		}

		var cart = await _context.Carts.FirstOrDefaultAsync(c => c.StandId == standId);
		if (cart is null)
		{
			// TODO: Add error message
			return NotFound();
		}
		return Ok(cart.ToDto());
	}

	[HttpPut]
	[ProducesResponseType<Cart>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CartDto>> UpsertCart(
		Guid eventId,
		Guid standId,
		[FromBody] CartDto request
	)
	{
		var stand = await _context.Stands.FirstOrDefaultAsync(s =>
			s.Id == standId && s.EventId == eventId
		);
		if (stand is null)
		{
			return NotFound();
		}
		if (standId != request.StandId)
		{
			return BadRequest();
		}

		var cart = await _context.Carts.FirstOrDefaultAsync(c => c.StandId == standId);
		var items = new List<CartItem>();
		foreach (var item in request.Items)
		{
			var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci =>
				ci.ProductId == item.ProductId && ci.CartId == cart!.Id
			);
			if (cartItem is not null)
			{
				cartItem.Quantity = item.Quantity;
				items.Add(cartItem);
				continue;
			}

			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
			if (product is null)
			{
				continue;
			}
			items.Add(
				new CartItem
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Product = product,
				}
			);
		}

		if (cart is not null && cart.IsCompleted == false)
		{
			cart.Items =
			[
				.. cart
					.Items.Concat(items)
					.GroupBy(x => x.ProductId)
					.Select(g => new CartItem
					{
						ProductId = g.Key,
						Quantity = g.Sum(x => x.Quantity),
						Product = g.First().Product,
					}),
			];
			_context.Carts.Update(cart);
			await _context.SaveChangesAsync();
			return Ok(cart.ToDto());
		}

		if (cart is not null)
		{
			_context.Carts.Remove(cart!);
			await _context.SaveChangesAsync();
		}

		var userId = Guid.Parse(User.FindFirst("user_id")?.Value ?? Guid.Empty.ToString())
			.ToString();
		var newCart = new Cart
		{
			Id = Guid.NewGuid(),
			UpdatedAt = DateTime.UtcNow,
			CreatedAt = DateTime.UtcNow,
			UserId = userId,
			StandId = standId,
			Stand = stand,
			Items = items,
		};
		newCart.Items = newCart
			.Items.Select(i =>
			{
				i.Cart = newCart;
				i.CartId = newCart.Id;
				return i;
			})
			.ToList();

		_context.Carts.Add(newCart);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetCart), new { eventId, standId }, newCart.ToDto());
	}
}
