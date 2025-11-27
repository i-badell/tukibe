using Api.Dto;
using Api.Models;

namespace Api.Helpers;

public static class StandMappers
{
	public static StandDto ToDto(this Stand stand) =>
		new()
		{
			Id = stand.Id,
			Name = stand.Name,
			ImageUrl = stand.ImageUrl,
			Products = [.. stand.Products.ToDto()],
		};

	public static IEnumerable<StandDto> ToDto(this IEnumerable<Stand> stands) =>
		stands.Select(x => x.ToDto());
}

public static class PorductsDto
{
	public static ProductDto ToDto(this Product product) =>
		new()
		{
			Id = product.Id,
			Price = product.Price,
			Name = product.Name,
			Description = product.Description ?? "",
			ImageUrl = product.ImageUrl,
		};

	public static IEnumerable<ProductDto> ToDto(this IEnumerable<Product> products) =>
		products.Select(x => x.ToDto());
}
