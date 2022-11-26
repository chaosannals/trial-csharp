using System.Text.Json;

namespace AspProxyDemo.Middlewares;

public class NotFoundMiddleware : IMiddleware
{
    private ILogger<NotFoundMiddleware> logger;

    public NotFoundMiddleware(ILogger<NotFoundMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
            if (context.Response.StatusCode == 404)
            {
                logger.LogWarning("404 path: {}", context.Request.Path);
            }
        }
        catch (Exception e)
        {
            logger.LogError("404: {}", e);
            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                new
                {
                    Code = -1,
                    Message = e.Message,
                    StackTrace = e.StackTrace,
                });
        }
    }
}
