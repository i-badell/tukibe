using Api.Context;
using Api.Middlewares;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// DI
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserService>();

// SQL Server
builder.AddSqlServerDbContext<ApiContext>("tuki-db");
builder.EnrichSqlServerDbContext<ApiContext>();


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

builder.Services.AddCors(options =>
 {
     options.AddPolicy("AllowSpecificOrigins", policy =>
     {
         policy.WithOrigins("http://localhost:3000", "http://localhost:4200") // Specify allowed origins
               .AllowAnyHeader()
               .AllowAnyMethod();
     });

     // You can also define a policy to allow all origins (for development/testing)
     options.AddPolicy("AllowAll", policy =>
     {
         policy.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
     });
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

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
