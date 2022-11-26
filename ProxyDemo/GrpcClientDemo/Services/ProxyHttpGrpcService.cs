using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcServerDemo;
using Microsoft.Extensions.Configuration;
using Grpc.Core;

namespace GrpcClientDemo.Services;

/// <summary>
/// 在 HTTP 下需要改变 代理的配置，强制使用 HTTP2 。
/// 详见 AspProxyDemo appsettings.json 的 Kestrel 配置。
/// </summary>
public class ProxyHttpGrpcService : BackgroundService
{
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private ILogger<ProxyHttpGrpcService> logger;

    public ProxyHttpGrpcService(IConfiguration config, ILogger<ProxyHttpGrpcService> logger)
    {
        var port = config.GetValue<int>("ProxyHttp:Port");
        var host = config.GetValue<string>("ProxyHttp:Host");

        channel = GrpcChannel.ForAddress($"http://{host}:{port}");
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
                    Name = "Proxy Http",
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
