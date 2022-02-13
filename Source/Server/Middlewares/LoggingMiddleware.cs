namespace MultiplayerSnake.Server
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Processing: " + context.Request.Path);

            await _next(context);

            _logger.LogInformation("Exited with status code: " + context.Response.StatusCode);
        }
    }
}
