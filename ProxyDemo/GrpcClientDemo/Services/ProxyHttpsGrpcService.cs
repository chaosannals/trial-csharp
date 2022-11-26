using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcServerDemo;
using Microsoft.Extensions.Configuration;
using Grpc.Core;

namespace GrpcClientDemo.Services;

/// <summary>
/// 代理必须 HTTPS HTTP/2，默认 AspNetCore 的配置就是如此。
/// </summary>
public class ProxyHttpsGrpcService : BackgroundService
{
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private ILogger<ProxyHttpsGrpcService> logger;

    public ProxyHttpsGrpcService(IConfiguration config, ILogger<ProxyHttpsGrpcService> logger)
    {
        var port = config.GetValue<int>("ProxyHttps:Port");
        var host = config.GetValue<string>("ProxyHttps:Host");
        // 使用本地调试的自签证书时，需要重置不验证证书的HTTPS处理
        var httpHandler = new HttpClientHandler();
        // Return `true` to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        channel = GrpcChannel.ForAddress(
            $"https://{host}:{port}",
            new GrpcChannelOptions {
                HttpHandler = httpHandler,
                
            }
        );
        client = new Greeter.GreeterClient(channel);
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(4000);
                var request = new HelloRequest
                {
                    Name = "Proxy Https",
                };
                var reply = await client.SayHelloAsync(request);
                logger.LogInformation("server: {}", reply);
            }
            catch (RpcException e)
            {
                logger.LogError("proxy rpc https: {}", e);
            }
            catch (Exception e)
            {
                logger.LogError("proxy https: {}", e);
            }
        }
    }
}
