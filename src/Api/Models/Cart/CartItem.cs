namespace Api.Models;

public sealed class CartItem
{
	public Guid Id { get; set; }
	public required Product Product { get; set; }
	public Guid ProductId { get; set; }
	public int Quantity { get; set; }
	public Guid CartId { get; set; }
	public required Cart Cart { get; set; }
}