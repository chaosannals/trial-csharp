using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using TxIMDemo.Services;

namespace TxIMDemo.Controllers;

[ApiController]
[Route("txim")]
public class TxIMController : ControllerBase
{
    private readonly TxIMService txIMService;

    public TxIMController(TxIMService txim)
    {
        txIMService = txim;
    }

    [HttpGet(Name = "signature")]
    public async Task<object> Signature(string username)
    {
        await Task.Yield();

        return new
        {
            Code = 0,
            Signature = txIMService.GenerateSignature(username),
        };
    }
}
