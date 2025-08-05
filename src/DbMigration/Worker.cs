using Microsoft.EntityFrameworkCore;
using Api.Context;
using DbMigration.Services;

namespace DbMigration;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            using var scope = _serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ClientContext>();
            await ctx.Database.MigrateAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5));
            var seedService = scope.ServiceProvider.GetRequiredService<ProductSeed>();
            Guid eventId;
            Guid.TryParse(_configuration["SeedEventId"], out eventId);
            await seedService.SeedSampleEventAsync(eventId);

            _logger.LogInformation("Migrations y seeding success");
        }
        catch (Exception e)
        {
            _logger.LogError("An error ocurred during migration", e);
        }
        Environment.Exit(0);
    }
}
