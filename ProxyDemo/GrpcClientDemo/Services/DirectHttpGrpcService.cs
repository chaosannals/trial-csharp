using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcServerDemo;
using Microsoft.Extensions.Configuration;

namespace GrpcClientDemo.Services;

public class DirectHttpGrpcService : BackgroundService
{
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private ILogger<DirectHttpGrpcService> logger;

    public DirectHttpGrpcService(IConfiguration config, ILogger<DirectHttpGrpcService> logger)
    {
        var port = config.GetValue<int>("DirectHttp:Port");
        var host = config.GetValue<string>("DirectHttp:Host");

        channel = GrpcChannel.ForAddress($"http://{host}:{port}");
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
                Name = "Direct Http",
            };
            var reply = await client.SayHelloAsync(request);
            logger.LogInformation("server: {}", reply);
        }
    }
}
