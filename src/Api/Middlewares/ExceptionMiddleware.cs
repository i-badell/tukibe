﻿using System.Net;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            var response = new
            {
                ErrorMsg = "Ocurrió un error interno. Intente más tarde."                
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
