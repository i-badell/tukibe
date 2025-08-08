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
        var dbEvent = _context.Events.FirstOrDefault(x => x.Id == eventId);
        if (dbEvent is not null)
        {
            return;
        }

        var eventEntity = new Event
        {
            Id = eventId,
            Name = $"Event {eventId}",
            ImageUrl = $"https://placehold.co/200"
        };

        var category_1 = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Comida"
        };
        
        var category_2 = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Bebida"                      
        };

        _context.Categories.Add(category_1);
        _context.Categories.Add(category_2);

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
                Name = standName,
                ImageUrl = $"https://placehold.co/200",
                Event = eventEntity,                
            };

            for (int j = 1; j <= 10; j++)
            {
                stand.Catalogs.Add(new Catalog
                {
                    Stand = stand,
                    Product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Producto {j} - {standName}",
                        Description = $"Delicioso {standName} número {j}",
                        Category = category_1,
                        ImageUrl = $"https://placehold.co/200"
                    },
                    InStock = true,
                    Price = 50 + j * 3
                });
            }

            for (int j = 1; j <= 10; j++)
            {
                stand.Catalogs.Add(new Catalog
                {
                    Stand = stand,
                    Product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Producto {j} - {standName}",
                        Description = $"Delicioso {standName} número {j}",
                        Category = category_2,
                        ImageUrl = $"https://placehold.co/200"
                    },
                    InStock = true,
                    Price = 30 + j * 3
                });
            }

            eventEntity.Stands.Add(stand);
        }

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
    }
}
