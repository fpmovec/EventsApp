using Web.Middleware;

namespace Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseRequestLogging(this WebApplication app)
            => app.UseMiddleware<RequestLoggingMiddleware>();

        public static void UseExceptionHandlingMiddleware(this WebApplication app)
            => app.UseMiddleware<EventsExceptionHandler>();
    }
}
