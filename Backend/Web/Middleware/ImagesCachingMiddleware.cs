namespace Web.Middleware
{
    public class ImagesCachingMiddleware(RequestDelegate next)
    {
        private readonly string[] ImageFormats = [".png", ".jpg", ".jpeg", ".bmp", ".webp"];
        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            if (ImageFormats.Any(path.EndsWith))
            {
                TimeSpan maxAge = new TimeSpan(7, 0, 0, 0); // 7 days
                context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
            }

            await next(context);
        }
    }
}
