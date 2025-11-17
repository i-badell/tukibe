namespace Api.Dto;

public sealed class CartItemDto
{
	public Guid ProductId { get; set; }
	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
}