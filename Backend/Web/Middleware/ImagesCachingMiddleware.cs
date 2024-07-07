namespace Web.Middleware
{
    public class ImagesCachingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".bmp"))
            {
                TimeSpan maxAge = new TimeSpan(7, 0, 0, 0); // 7 days
                context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
            }

            await next(context);
        }
    }
}
