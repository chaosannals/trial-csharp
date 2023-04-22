

using tencentyun;

namespace TxIMDemo.Services;

public class TxIMService
{
    private TLSSigAPIv2 signer;
    private ILogger<TxIMService> logger;

    public TxIMService(IConfiguration configuration, ILogger<TxIMService> logger)
    {
        this.logger = logger;
        var appId = configuration.GetValue<int>("TxIM:AppId", 0);
        var appKey = configuration.GetValue("TxIM:AppKey", "");
        signer = new TLSSigAPIv2(appId, appKey);
        logger.LogInformation("appid: {} ; appKey: {}", appId, appKey);
    }

    public string GenerateSignature(string username)
    {
        return signer.GenSig(username);
    }
}
