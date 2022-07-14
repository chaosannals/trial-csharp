using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HttpServer.Utilities;

namespace HttpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly ILogger<SignController> logger;
        private SignManager sm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sm"></param>
        public SignController(ILogger<SignController> logger, SignManager sm)
        {
            this.logger = logger;
            this.sm=sm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [Route("ack")]
        [HttpPost]
        public async Task<object> SignAck(string account)
        {
            var token = await sm.Ack(account);
            return new
            {
                Code = 0,
                Token = token,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> SignIn(string token)
        {
            var proof = await sm.Sign(token);

            return new
            {
                Code = 0,
                Proof = proof,
            };
        }

        [Route("decrypt")]
        [HttpPost]
        public async Task<object> Decrypt(string key, string signature)
        {
            var token = SignManager.Decrypt(key, signature);
            return new
            {

            };
        }

        [Route("encrypt")]
        [HttpPost]
        public async Task<object> Encrypt(string token)
        {
            return new
            {

            };
        }
    }
}
