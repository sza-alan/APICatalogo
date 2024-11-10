using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // executa depois do Action
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("##################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"StatusCode: {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("##################################################");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // executa antes do Action
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("##################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
        _logger.LogInformation("##################################################");
    }
}
