namespace Api.Dto;

public sealed class CartDto
{
	public Guid Id { get; set; }
	public Guid StandId { get; set; }
	public List<CartItemDto> Items { get; set; } = [];
}