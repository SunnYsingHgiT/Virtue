using System.Diagnostics;

namespace Virtue.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Logging logic before passing to the next middleware
            Debug.WriteLine($"Handling request: {context.Request.Method} {context.Request.Path}");

            await _next(context);  // Call the next middleware in the pipeline

            // Logging logic after the next middleware has handled the request
            Debug.WriteLine($"Finished handling request.");
        }
    }
}
