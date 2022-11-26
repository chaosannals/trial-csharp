using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using GrpcServerDemo;
using Microsoft.Extensions.Logging;

namespace GrpcClientDemo.Services;

public class DirectHttpsGrpcService : BackgroundService
{
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private ILogger<DirectHttpsGrpcService> logger;

    public DirectHttpsGrpcService(IConfiguration config, ILogger<DirectHttpsGrpcService> logger)
    {
        var port = config.GetValue<int>("DirectHttps:Port");
        var host = config.GetValue<string>("DirectHttps:Host");

        // 使用本地调试的自签证书时，需要重置不验证证书的HTTPS处理
        var httpHandler = new HttpClientHandler();
        // Return `true` to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        channel = GrpcChannel.ForAddress($"https://{host}:{port}", new GrpcChannelOptions { HttpHandler = httpHandler });
        client = new Greeter.GreeterClient(channel);
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(4000);
            var request = new HelloRequest
            {
                Name = "Direct Https",
            };
            var reply = await client.SayHelloAsync(request);
            logger.LogInformation("server: {}", reply);
        }
    }
}
