using Api.Context;
using Api.Dto;
using Api.Models;
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
                Stands = e.Stands
                    .Select(s => new StandDto
                    {
                        StandId = s.Id,
                        Name = s.Name,
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
                    .ToList()
            })
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
