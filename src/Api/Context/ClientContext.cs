using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Context;

public class ClientContext : DbContext
{
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stand> Stands { get; set; }

    public ClientContext(DbContextOptions<ClientContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stand>()
            .HasMany(s => s.Catalogs)
            .WithOne(c => c.Stand)
            .HasForeignKey(c => c.StandId)
            .IsRequired(false);

        modelBuilder.Entity<Catalog>()
          .HasOne(e => e.Event)
          .WithOne(c => c.Catalog)
          .HasForeignKey<Catalog>(x => x.EventId)
          .IsRequired(false);

        modelBuilder.Entity<Product>()
          .Property(m => m.Price)
          .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
               .HasMany(p => p.Catalogs)
               .WithMany(c => c.Products)
               .UsingEntity(join => join.ToTable("ProductCatalogs"));

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Stands)
            .WithMany(s => s.Events)
            .UsingEntity(join => join.ToTable("EventStands"));

        base.OnModelCreating(modelBuilder);
    }
}
