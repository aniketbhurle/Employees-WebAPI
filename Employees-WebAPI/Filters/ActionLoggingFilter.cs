using Microsoft.AspNetCore.Mvc.Filters;

namespace Employees_WebAPI.Filters;

public class ActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ActionLoggingFilter> _logger;

    public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation($"Action {actionName} finished..");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation($"Action {actionName} started..");
    }
}
