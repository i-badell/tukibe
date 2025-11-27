namespace Api.Models;

public sealed class Cart
{
	public Guid Id { get; set; }
	public required string UserId { get; set; }
	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; set; }
	public bool IsCompleted { get; set; }

	public Guid StandId { get; set; }
	public required Stand Stand { get; set; }

	public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
