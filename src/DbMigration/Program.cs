using Api.Context;
using DbMigration;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

builder.Services.AddHostedService<Worker>();
builder.AddSqlServerDbContext<ClientContext>("tuki-db");
var host = builder.Build();
host.Run();
