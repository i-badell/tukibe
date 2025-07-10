using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("tuki-db").WithLifetime(ContainerLifetime.Persistent);
var db = sql.AddDatabase("client");

var migration = builder.AddProject<Projects.DbMigration>("DbMigration")
  .WithReference(sql)
  .WaitFor(db);

builder.AddProject<Projects.Api>("Api")
  .WithReference(sql)
  .WaitFor(migration);

builder.Build().Run();
