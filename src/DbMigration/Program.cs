using Api.Context;
using DbMigration;
using DbMigration.Services;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

builder.AddSqlServerDbContext<ApiContext>("tuki-db");
builder.Services.AddScoped<TestEventSeedService>();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();
