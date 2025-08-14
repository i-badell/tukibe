using Api.Context;
using Api.Dto;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Api.Services;

public class EventService : IEventService
{
    private readonly ClientContext _context;

    public EventService(ClientContext context)
    {
        _context = context;
    }
    public async Task<StandResponse?> GetStandData(Guid eventId, Guid standId)
    {
        var e = await _context.Events
            .Where(e => e.Id == eventId)
            .FirstOrDefaultAsync();

        if (e is null) {
            return null;
        }

        return await _context.Stands            
            .Where(s => s.Id == standId)
            .Select(s => new StandResponse
            {
                EventId = eventId,
                StandId = standId,
                StandName = s.Name,
                StandImageUrl = s.ImageUrl,
                Products = s.Catalogs                    
                    .Select(c => new ProductDto
                    {
                        ProductId = c.Product.Id,
                        Name = c.Product.Name,
                        Description = c.Product.Description,
                        ImageUrl = c.Product.ImageUrl,
                        Price = c.Price
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
    public async Task<EventStandsResponse?> GetEventStands(Guid eventId)
    {
        var e = await _context.Events
            .Where(e => e.Id == eventId)
            .Include(e => e.Stands)
            .ThenInclude(s => s.Catalogs)
            .ThenInclude(c => c.Product)
            .FirstOrDefaultAsync();

        if (e is null)
        {
            return null;
        }

        var stands = e.Stands
            .Select(s => new StandDto
            {
                StandId = s.Id,
                Name = s.Name,
                StandImageUrl = s.ImageUrl,
                Products = s.Catalogs
                    .OrderByDescending(c => c.Price)
                    .Take(5)
                    .Select(c => new ProductDto
                    {
                        ProductId = c.Product.Id,
                        Name = c.Product.Name,
                        Description = c.Product.Description,
                        ImageUrl = c.Product.ImageUrl,
                        Price = c.Price
                    })
                    .ToList()
            }).ToList();

        EventStandsResponse response = new EventStandsResponse
        {
            EventId = e.Id,
            EventName = e.Name,
            EventImageUrl = e.ImageUrl,
            Stand = stands.Count == 1 ? stands.First() : null,
            Stands = stands.Count > 1 ? stands : []
        };

        return response;
    }

    public async Task<Event?> GetEventById(Guid eventId)
    {
        return await _context.Events
            .Where(e => e.Id == eventId)
            .FirstOrDefaultAsync();
    }
}

public static class Mappers
{
    //public static List<ProductDto> ToDto(this IEnumerable<Product> products) =>
    //  products
    //    .Where(p => p.InStock)
    //    .Select(p => p.ToDto())
    //    .ToList();


    //public static ProductDto ToDto(this Product product) => new ProductDto
    //{
    //    ProductId = product.Id,
    //    Name = product.Name,
    //    Description = product.Description,
    //    Price = product.Price,
    //    ImageUrl = product.ImageUrl
    //};


    public static List<CategoryDto> ToDto(this IEnumerable<Category> Categories) =>
     Categories
       .Select(c => c.ToDto())
       .ToList();

    public static CategoryDto ToDto(this Category category) => new CategoryDto
    {
        Id = category.Id,
        Name = category.Name,
    };
}
