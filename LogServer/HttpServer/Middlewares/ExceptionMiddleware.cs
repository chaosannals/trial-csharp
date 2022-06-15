using System.Text.Json;

namespace HttpServer.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private ILogger<LogAckMiddleware> logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<LogAckMiddleware> logger)
    {
        this.next=next;
        this.logger=logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch(Exception e)
        {
            logger.LogError("err: {0}", e);
            ctx.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                ctx.Response.Body,
                new
                {
                    Code = -1,
                    Message = e.Message,
                    StackTrace = e.StackTrace,
                });
        }
    }
}
