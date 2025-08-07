using Api.Context;
using Api.Services.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// DI
builder.Services.AddScoped<IEventService, EventService>();

// SQL Server
builder.AddSqlServerDbContext<ClientContext>("tuki-db");
builder.EnrichSqlServerDbContext<ClientContext>();


// AUthorization
builder.Services
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = config["Auth0Config:Authority"];
        options.Audience = config["Auth0Config:Audience"];
    });

// OpenApi 
builder.Services.AddOpenApi("v1", opt => opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTheme(ScalarTheme.Moon)
          .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
          .AddPreferredSecuritySchemes("Bearer")
          .WithPersistentAuthentication();

        opt.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = ["Bearer"]
        };
    });
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
