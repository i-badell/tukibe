using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("tuki-db").WithLifetime(ContainerLifetime.Persistent);
var db = sql.AddDatabase("client");


var migration = builder.AddProject<Projects.DbMigration>("DbMigration")
  .WithReference(sql)
  .WaitFor(db);

var api = builder.AddProject<Projects.Api>("Api")
  .WithReference(sql)
  .WaitFor(migration); ;

builder.AddScalarApiReference(opt =>
{
    opt.WithTheme(ScalarTheme.Moon)
      .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
      .AddPreferredSecuritySchemes("Bearer")
      .WithPersistentAuthentication();

    opt.Authentication = new ScalarAuthenticationOptions
    {
        PreferredSecuritySchemes = ["Bearer"]
    };
}).WaitFor(api).WithApiReference(api, opt =>
{
    opt.AddDocument("v1", title: "Tuki Api Docs", routePattern: "/openapi/{documentName}.json");
});

builder.Build().Run();
