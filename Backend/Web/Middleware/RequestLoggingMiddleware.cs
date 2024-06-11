using Microsoft.AspNetCore.Mvc;

namespace Web.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, [FromServices] ILogger<RequestLoggingMiddleware> logger)
        {
            logger.LogInformation($"Request to {context.Request.Path}. Method \"{context.Request.Method}\"");

            await _next(context);

            int responseCode = context.Response.StatusCode;

            LogLevel logLevel = GetLoggerLevel(responseCode);

            logger.Log(logLevel, $"Response status code: {responseCode}");
        }

        private LogLevel GetLoggerLevel(int statusCode) => statusCode switch
        {
            >= 100 and < 400 => LogLevel.Information,
            >= 400 and < 500 => LogLevel.Error,
            _ => LogLevel.Critical
        };
    }
}
