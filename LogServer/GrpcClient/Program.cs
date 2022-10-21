using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcClient.Services;
using GrpcClient;
using Grpc.Core;
using Grpc.Net.Client;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "GRPC_CLIENT_");
    })
    .ConfigureLogging((hc, cl) =>
    {
        cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
        cl.AddConsole();
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddSingleton<ChannelBase>(op =>
        {
            var conf = op.GetRequiredService<IConfiguration>();
            var host = conf.GetValue<string>("GrpcServer:Host");
            var port = conf.GetValue<int>("GrpcServer:Port");
            return GrpcChannel.ForAddress($"https://{host}:{port}");
        });
        services.AddSingleton<Greeter.GreeterClient>();
        services.AddHostedService<GreeterClientService>();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();
