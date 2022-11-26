using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GrpcClientDemo.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "GRPC_CLIENT_DEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddHostedService<ProxyHttpGrpcService>();
        services.AddHostedService<ProxyHttpsGrpcService>();
        services.AddHostedService<DirectHttpGrpcService>();
        services.AddHostedService<DirectHttpsGrpcService>();
    })
    .ConfigureLogging((hc, cl) =>
    {
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();
