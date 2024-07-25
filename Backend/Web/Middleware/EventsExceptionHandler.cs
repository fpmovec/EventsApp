using Web.DTO;

namespace Web.Middleware
{
    public class EventsExceptionHandler(RequestDelegate _next, ILogger<EventsExceptionHandler> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            _logger.LogCritical($"Exception was thrown! Exception message: {ex.Message} \n Inner exception message: {ex.InnerException?.Message}");
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new ErrorModel(statusCode, ex.Message));
        }
    }
}
