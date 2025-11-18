using Api.Dto;
using Api.Models;

namespace Api.Helpers;

public static class CartMapper
{
	public static CartDto ToDto(this Cart cart) => new()
	{
		Id = cart.Id,
		StandId = cart.StandId,
		Items = [.. cart.Items.Select(i => new CartItemDto
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity,
				UnitPrice = i.Product.Price
			})]
	};
}