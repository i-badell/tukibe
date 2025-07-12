using Api.Context;
using DbMigration;
using DbMigration.Services;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

builder.Services.AddScoped<ProductSeed>();
builder.Services.AddHostedService<Worker>();
builder.AddSqlServerDbContext<ClientContext>("tuki-db");
var host = builder.Build();
host.Run();
