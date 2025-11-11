using System.Net;
using System.Text.Json;

namespace TaskManagementAPI.API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorDetails = new ErrorDetails
        {
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path
        };

        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                errorDetails.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails.Message = "Invalid request";
                errorDetails.Details = exception.Message;
                break;

            case UnauthorizedAccessException:
                errorDetails.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorDetails.Message = "Unauthorized access";
                errorDetails.Details = exception.Message;
                break;

            case KeyNotFoundException:
                errorDetails.StatusCode = (int)HttpStatusCode.NotFound;
                errorDetails.Message = "Resource not found";
                errorDetails.Details = exception.Message;
                break;

            default:
                errorDetails.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetails.Message = "An error occurred while processing your request";
                errorDetails.Details = exception.Message;
                break;
        }

        context.Response.StatusCode = errorDetails.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorDetails, options);
        await context.Response.WriteAsync(json);
    }
}
