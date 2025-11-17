using Api.Context;
using Api.Dto;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class EventService : IEventService
{
    private readonly ApiContext _context;

    public EventService(ApiContext context)
    {
        _context = context;
    }
    public async Task<StandResponse?> GetStandData(Guid eventId, Guid standId)
    {
        var e = await _context.Events
            .Where(e => e.Id == eventId)
            .FirstOrDefaultAsync();

        if (e is null)
        {
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
                Products = s.Products.Select(p => new ProductDto
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    public async Task<EventStandsResponse?> GetEventStands(Guid eventId)
    {
        var e = await _context.Events
            .Where(e => e.Id == eventId)
            .Include(e => e.Stands)
            .ThenInclude(c => c.Products)
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
                Products = s.Products
                    .OrderByDescending(c => c.Price)
                    .Take(5)
                    .Select(c => new ProductDto
                    {
                        ProductId = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        Price = c.Price
                    })
                    .ToList()
            }).ToList();

        var response = new EventStandsResponse
        {
            EventId = e.Id,
            EventName = e.Name,
            EventImageUrl = e.ImageUrl,
            Stand = stands.Count == 1 ? stands.First() : null,
            Stands = stands.Count > 1 ? stands : []
        };

        return response;
    }
    public async Task<EventStandsResponse?> GetEventStandsSummary(Guid eventId)
    {
        var e = await _context.Events
            .Where(e => e.Id == eventId)
            .Include(e => e.Stands)
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
                StandImageUrl = s.ImageUrl
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