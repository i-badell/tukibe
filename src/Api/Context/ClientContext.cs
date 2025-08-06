using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Context;

public class ClientContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stand> Stands { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public ClientContext(DbContextOptions<ClientContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Stands)
            .WithOne(s => s.Event);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Catalog>()
            .HasKey(c => new { c.StandId, c.ProductId });

        modelBuilder.Entity<Catalog>()
          .Property(c => c.Price)
          .HasPrecision(18, 2);

        modelBuilder.Entity<Catalog>()
            .HasOne(c => c.Stand)
            .WithMany(s => s.Catalogs)
            .HasForeignKey(c => c.StandId);

        modelBuilder.Entity<Catalog>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Catalogs)
            .HasForeignKey(c => c.ProductId);

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
