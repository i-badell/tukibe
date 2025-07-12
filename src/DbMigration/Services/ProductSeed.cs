using Api.Context;
using Api.Models;

namespace DbMigration.Services;

public class ProductSeed
{
    private readonly ClientContext _context;

    public ProductSeed(ClientContext context)
    {
        _context = context;
    }

    public async Task SeedSampleEventAsync(Guid eventId)
    {
        var eventEntity = new Event
        {
            Id = eventId
        };

        var mainCatalog = new Catalog
        {
            Id = Guid.NewGuid()
        };
        for (int i = 1; i <= 10; i++)
        {
            mainCatalog.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Producto {i}",
                Description = $"Descripción del producto {i}",
                InStock = true,
                Price = 100 + i * 5,
                ImageUrl = $"https://example.com/images/producto{i}.jpg"
            });
        }
        eventEntity.Catalogs.Add(mainCatalog);
        mainCatalog.Events.Add(eventEntity);

        var standNames = new[]
        {
                "Stand de Jugos",
                "Stand de Empanadas",
                "Stand de Postres"
            };

        foreach (var standName in standNames)
        {
            var stand = new Stand
            {
                Id = Guid.NewGuid(),
                Name = standName
            };

            var standCatalog = new Catalog
            {
                Id = Guid.NewGuid(),
                Stand = stand
            };

            for (int j = 1; j <= 5; j++)
            {
                standCatalog.Products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Producto {j} - {standName}",
                    Description = $"Delicioso {standName} número {j}",
                    InStock = true,
                    Price = 50 + j * 3,
                    ImageUrl = $"https://example.com/images/{standName.Replace(" ", "").ToLower()}{j}.jpg"
                });
            }

            stand.Catalogs.Add(standCatalog);

            eventEntity.Stands.Add(stand);
            eventEntity.Catalogs.Add(standCatalog);

            standCatalog.Events.Add(eventEntity);
        }

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
    }
}
