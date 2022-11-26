using Microsoft.AspNetCore.Mvc;

namespace AspServerDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class YetDemoApiController : ControllerBase
{
    [Route("DoSomeThing")]
    [HttpGet]
    public async Task<object> Index()
    {
        await Task.Yield();
        return new
        {
            Code = 0,
            Message = "Ok",
        };
    }
}
