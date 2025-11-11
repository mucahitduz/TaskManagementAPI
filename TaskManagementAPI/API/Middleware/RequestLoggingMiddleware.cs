using System.Diagnostics;
using System.Text;

namespace TaskManagementAPI.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await LogRequest(context);

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
            stopwatch.Stop();

            await LogResponse(context, stopwatch.ElapsedMilliseconds);
        }
        finally
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        var builder = new StringBuilder();
        builder.AppendLine($"HTTP Request: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
        builder.AppendLine($"Host: {context.Request.Host}");
        builder.AppendLine($"Content-Type: {context.Request.ContentType}");

        if (context.Request.ContentLength > 0)
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(body))
            {
                builder.AppendLine($"Body: {body}");
            }
        }

        _logger.LogInformation(builder.ToString());
    }

    private async Task LogResponse(HttpContext context, long elapsedMilliseconds)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"HTTP Response: {context.Response.StatusCode}");
        builder.AppendLine($"Content-Type: {context.Response.ContentType}");
        builder.AppendLine($"Duration: {elapsedMilliseconds}ms");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (!string.IsNullOrEmpty(responseBody))
        {
            builder.AppendLine($"Body: {responseBody}");
        }

        _logger.LogInformation(builder.ToString());
    }
}
