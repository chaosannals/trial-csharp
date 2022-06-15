using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace HttpServer;

public class ControllerExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled == false)
        {
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/json;charset=utf-8",
                Content = JsonSerializer.Serialize(new
                {
                    Code = -2,
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace,
                }),
            };
        }
        await Task.CompletedTask;
    }
}
