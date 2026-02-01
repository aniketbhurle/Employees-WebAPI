using System.Diagnostics;
using Serilog;

namespace Employees_WebAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next; 
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var time = Stopwatch.StartNew();

        //Console.WriteLine($"Request Path: {context.Request.Path}");
        _logger.LogInformation($"Request Path: {context.Request.Path}");

        await _next(context);

        time.Stop();

        //Console.WriteLine($"Request took : {time.ElapsedMilliseconds} ms \n");

        _logger.LogInformation( "Request {Method} {Path} executed in {ElapsedMilliseconds} ms",
                                context.Request.Method,
                                context.Request.Path,
                                time.ElapsedMilliseconds);

    }
}

