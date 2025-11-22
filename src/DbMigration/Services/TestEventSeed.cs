using Api.Context;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DbMigration.Services;

public class TestEventSeedService
{
    private readonly ApiContext _context;

    public TestEventSeedService(ApiContext context)
    {
        _context = context;
    }

    public async Task SeedSampleEventAsync(Guid eventId)
    {
        // Clear existing data
        await _context.Products.ExecuteDeleteAsync();
        await _context.Stands.ExecuteDeleteAsync();
        await _context.Events.ExecuteDeleteAsync();

        // Create event
        var @event = new Event
        {
            Id = eventId,
            Name = "Feria de Comida 2024",
            ImageUrl = "https://placehold.co/400"
        };
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        // Stand names
        var standNames = new[]
        {
            "Tacos El Gobernador",
            "Pizzería La Nonna",
            "Comida Rápida Doña Maria",
            "Postres Dulce Tentación"
        };

        var productNames = new[]
        {
            "Tacos de Carnitas",
            "Tacos de Pollo",
            "Tacos al Pastor",
            "Enchiladas Verdes",
            "Quesadillas de Queso",
            "Chiles Rellenos",
            "Tamales Oaxaqueños",
            "Sopes de Chicharrón",
            "Flautas Doradas",
            "Ceviche Mixto"
        };

        var descriptions = new[]
        {
            "Delicioso platillo preparado con ingredientes frescos",
            "Receta tradicional de la casa",
            "Hecho con las mejores carnes selectas",
            "Acompañado con guarniciones especiales",
            "Preparado al momento para tu disfrute",
            "Exquisito sabor auténtico",
            "Receta familiar transmitida por generaciones",
            "Ingredientes de primera calidad",
            "Listo para satisfacer tu paladar",
            "Especialidad de nuestro restaurante"
        };

        foreach (var standName in standNames)
        {
            var stand = new Stand
            {
                Id = Guid.NewGuid(),
                Name = standName,
                ImageUrl = "https://placehold.co/400",
                EventId = eventId,
                Event = @event
            };
            _context.Stands.Add(stand);
            await _context.SaveChangesAsync();

            // Add 10 products per stand
            for (int i = 0; i < 10; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productNames[i],
                    Description = descriptions[i],
                    ImageUrl = "https://placehold.co/400",
                    StandId = stand.Id,
                    Stand = stand,
                    Price = 45.00m + (i * 5.50m),
                    IsAvailable = true
                };
                _context.Products.Add(product);
            }
            await _context.SaveChangesAsync();
        }
    }
}
