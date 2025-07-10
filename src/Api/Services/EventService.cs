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

    public async Task<EventProducts?> GetEventProducts(Guid eventId)
    {
        return await _context.Events
            .Where(e => e.Id == eventId)
            .Select(e => new EventProducts
            {
                EventId = e.Id,
                Catalogs = e.Catalogs
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
                    .ToList(),
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
