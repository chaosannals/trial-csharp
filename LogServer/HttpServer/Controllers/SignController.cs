using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly ILogger<SignController> logger;
        private SignManager sm;

        public SignController(ILogger<SignController> logger, SignManager sm)
        {
            this.logger = logger;
            this.sm=sm;
        }

        [HttpPost]
        public async Task<object> SignIn()
        {

            return new
            {
                Code = 0,
            };
        }
    }
}
