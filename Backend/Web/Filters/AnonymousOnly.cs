using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousOnly : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }

            return Task.CompletedTask;
        }
    }
}
