using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApiContext : DbContext
{
	public DbSet<Event> Events { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Stand> Stands { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Cart> Carts { get; set; }
	public DbSet<CartItem> CartItems { get; set; }
	public DbSet<Notification> Notifications { get; set; }

	public ApiContext(DbContextOptions<ApiContext> options)
		: base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("client-test");

		modelBuilder.Entity<User>().HasKey(x => x.Id);

		modelBuilder.Entity<Event>().HasKey(x => x.Id);

		modelBuilder.Entity<Event>().HasMany(e => e.Stands).WithOne(s => s.Event);

		modelBuilder.Entity<Stand>().HasMany(s => s.Products).WithOne(p => p.Stand);

		modelBuilder.Entity<Stand>().HasKey(x => x.Id);

		// Cart configuration
		modelBuilder.Entity<Cart>().HasKey(c => c.Id);

		modelBuilder
			.Entity<Cart>()
			.HasMany(c => c.Items)
			.WithOne(i => i.Cart)
			.HasForeignKey(i => i.CartId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder
			.Entity<Cart>()
			.HasOne(c => c.Stand)
			.WithMany()
			.HasForeignKey(c => c.StandId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder
			.Entity<Product>()
			.Property(x => x.Price)
			.HasColumnType("decimal")
			.HasPrecision(5, 2);

		modelBuilder.Entity<CartItem>().HasKey(i => i.Id);

		modelBuilder
			.Entity<CartItem>()
			.HasOne(i => i.Product)
			.WithMany(p => p.CartItems)
			.HasForeignKey(i => i.ProductId)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder
			.Entity<CartItem>()
			.HasOne(i => i.Cart)
			.WithMany(c => c.Items)
			.HasForeignKey(i => i.CartId)
			.OnDelete(DeleteBehavior.Cascade);

		// Notification configuration
		modelBuilder
			.Entity<Notification>()
			.HasOne(n => n.Event)
			.WithMany()
			.HasForeignKey(n => n.EventId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder
			.Entity<Notification>()
			.HasOne(n => n.User)
			.WithMany(u => u.Notifications)
			.HasForeignKey(n => n.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		base.OnModelCreating(modelBuilder);
	}
}
