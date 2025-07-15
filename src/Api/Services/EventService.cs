using Api.Context;
using Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class EventService : IEventService
{
    private readonly ClientContext _context;

    public EventService(ClientContext context)
    {
        _context = context;
    }

    public async Task<ProductResponse?> GetEventProducts(Guid eventId)
    {
        // TODO: Add mappers for dtos
        return await _context.Events
            .Where(e => e.Id == eventId)
            .Select(e => new ProductResponse
            {
                EventId = e.Id,
                Catalogs = e.Catalog == null ? null : new CatalogDto
                {
                    CatalogId = e.Catalog.Id,
                    Products = e.Catalog.Products
                        .Select(p => new ProductDto
                        {
                            ProductId = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            InStock = p.InStock,
                            Price = p.Price,
                            ImageUrl = p.ImageUrl
                        })
                        .ToList()

                },
                Stands = e.Stands
                    .Select(s => new StandDto
                    {
                        StandId = s.Id,
                        Name = s.Name,
                        Catalogs = s.Catalogs
                            .Select(c => new CatalogDto
                            {
                                CatalogId = c.Id,
                                Products = c.Products
                                    .Select(p => new ProductDto
                                    {
                                        ProductId = p.Id,
                                        Name = p.Name,
                                        Description = p.Description,
                                        InStock = p.InStock,
                                        Price = p.Price,
                                        ImageUrl = p.ImageUrl
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}
