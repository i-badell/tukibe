namespace Api.Models;

public class Product
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public required string ImageUrl { get; set; }
	public decimal Price { get; set; }
	public bool IsAvailable { get; set; }

	public Guid StandId { get; set; }
	public required Stand Stand { get; set; }

	public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
