using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace luchito_net.Middleware;

public class Middleware(ILogger<Middleware> logger) : IMiddleware, IActionFilter
{
    private void LogRequestedPath(HttpContext context, Exception? exception)
    {
        string ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        string deviceInfo = context.Request.Headers["User-Agent"].ToString();
        if (exception == null)
        {
            logger.LogInformation("The user with IP: {IPAddress} and device: {DeviceInfo} requested path: {Path} at {Timestamp}", ipAddress, deviceInfo, context.Request.Path, DateTime.UtcNow);
            return;
        }
        logger.LogError(exception, "The user with IP: {IPAddress} and device: {DeviceInfo} requested path: {Path} responded {StatusCode} at {Timestamp}", ipAddress, deviceInfo, context.Request.Path, context.Response.StatusCode, DateTime.UtcNow);
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
            LogRequestedPath(context, null);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        LogRequestedPath(context, exception);
        if (exception.Message.Contains("not found"))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(exception.Message);
        }
        else if (exception is DbException)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("A database error occurred. Please try again later.");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { Errors = errors });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after the action executes
    }
}