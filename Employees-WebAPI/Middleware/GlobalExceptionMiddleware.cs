using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Employees_WebAPI.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Unhandled expection occured");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "",
            Title = "",
            Status = StatusCodes.Status500InternalServerError,
            Detail = ""
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status.Value;

        var json = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsJsonAsync(json);
    }
}
