using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrpcClient.Services;

public class GreeterClientService : BackgroundService
{
    private Greeter.GreeterClient greeterClient;
    private ILogger<GreeterClientService> logger;

    public GreeterClientService(Greeter.GreeterClient greeterClient, ILogger<GreeterClientService> logger)
    {
        this.greeterClient=greeterClient;
        this.logger=logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var reply = await greeterClient.SayHelloAsync(
                new HelloRequest { Name = "TestClient Greeter "}
            );
            logger.LogInformation("Greeting Client: {0}", reply.Message);
            await Task.Delay(1000);
        }
    }
}
