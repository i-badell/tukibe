namespace Api.Models;

public sealed class CartItem
{
	public Guid Id { get; set; }
	public Product Product { get; set; }
	public Guid ProductId { get; set; }
	public int Quantity { get; set; }
	public Guid CartId { get; set; }
	public Cart Cart { get; set; }
}