using System.Diagnostics;

namespace Employees_WebAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next; 
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var time = Stopwatch.StartNew();

        Console.WriteLine($"Request Path: {context.Request.Path}");

        await _next(context);

        time.Stop();

        Console.WriteLine($"Request took : {time.ElapsedMilliseconds} ms \n");

    }
}

