using Microsoft.EntityFrameworkCore;
using Api.Models;

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
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Stands)
            .WithOne(s => s.Event);

        modelBuilder.Entity<Stand>()
            .HasMany(s => s.Products)
            .WithOne(p => p.Stand);

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Cart);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Stand)
            .WithMany()
            .HasForeignKey(c => c.StandId);

        modelBuilder.Entity<CartItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Event)
            .WithMany()
            .HasForeignKey(n => n.EventId);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId);

        base.OnModelCreating(modelBuilder);
    }
}
