using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "PROXY_DEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddReverseProxy()
            .LoadFromConfig(hc.Configuration.GetSection("ReverseProxy")); ;
    })
    .ConfigureLogging((hc, cl) =>
    {

    })
    .UseConsoleLifetime()
    .Build();

host.Run();
