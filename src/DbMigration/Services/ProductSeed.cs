using Api.Context;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DbMigration.Services;

public class ProductSeed
{
    private readonly ApiContext _context;

    public ProductSeed(ApiContext context)
    {
        _context = context;
    }

    public async Task SeedSampleEventAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}
