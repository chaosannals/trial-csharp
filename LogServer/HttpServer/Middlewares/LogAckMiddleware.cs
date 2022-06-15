using HttpServer.Attributters;

namespace HttpServer.Middlewares;

public class LogAckMiddleware
{
    private readonly RequestDelegate next;
    private ILogger<LogAckMiddleware> logger;
    private SignManager sm;

    public LogAckMiddleware(RequestDelegate next, ILogger<LogAckMiddleware> logger, SignManager sm)
    {
        this.next=next;
        this.logger = logger;
        this.sm=sm;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();
        var la = endpoint?.Metadata.GetMetadata<LogAckAttribute>();
        if (la != null)
        {
            logger.LogInformation("ack: {0}", endpoint!.DisplayName);
        }
        await next(ctx);
    }
}
