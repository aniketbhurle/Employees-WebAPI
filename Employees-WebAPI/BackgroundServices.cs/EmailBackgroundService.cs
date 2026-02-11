
using Microsoft.Extensions.Logging;

namespace Employees_WebAPI.BackgroundServices.cs;

public class EmailBackgroundService : BackgroundService
{
    private readonly ILogger<EmailBackgroundService> _logger;
    public EmailBackgroundService(ILogger<EmailBackgroundService> logger)
    {
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Background Task Started...");

        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Background Service Running at: {DateTimeOffset.Now}");

            await Task.Delay(TimeSpan.FromSeconds(10),stoppingToken);
        }
               
        _logger.LogInformation("Email Background Service Stopped...");
    }
}
