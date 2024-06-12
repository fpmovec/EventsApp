using Domain.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Web.ViewModels;

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
            int statusCode = (int)StatusCodes.Status500InternalServerError;

            _logger.LogCritical($"Exception was thrown! Exception message: {ex.Message}");

            string errorResponse =  JsonConvert.SerializeObject(new ErrorModel(statusCode, ex.Message));

            await context.Response.WriteAsync(errorResponse);
        }
    }
}
