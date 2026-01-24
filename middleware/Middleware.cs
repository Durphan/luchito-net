using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;

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
        if (exception is PostgresException)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("A database error occurred. Please try again later.");
        }
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(field => field.Value?.Errors.Count > 0)
                .ToDictionary(
                    field => field.Key,
                    field => field.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { Errors = errors });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            LogRequestedPath(context.HttpContext, context.Exception);
        }
    }
}