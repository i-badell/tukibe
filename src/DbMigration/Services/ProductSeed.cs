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
            ImageUrl = $"https://example.com/images/event_{eventId}.jpg"
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
                ImageUrl = $"https://example.com/images/{standName.Replace(" ", "").ToLower()}.jpg",
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
                        ImageUrl = $"https://example.com/images/{standName.Replace(" ", "").ToLower()}_{j}.jpg"
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
                        ImageUrl = $"https://example.com/images/{standName.Replace(" ", "").ToLower()}_{j}.jpg"
                    },
                    InStock = true,
                    Price = 30 + j * 3
                });
            }

            eventEntity.Stands.Add(stand);
        }

        _context.Events.Add(eventEntity);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Auth0Id = "auth0|demo-user",
            IsDeleted = false,            
        };
        _context.Users.Add(user);

        for (int i = 1; i <= 10; i++)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Event = eventEntity,
                User = user,
                Title = $"Title Notification #{i}",
                Message = $"Message Notification #{i}",
                CreatedAt = DateTime.UtcNow,
                IsRead = i < 7,
                IsDeleted = i < 3
            };

            _context.Notifications.Add(notification);
        }

        await _context.SaveChangesAsync();
    }
}
