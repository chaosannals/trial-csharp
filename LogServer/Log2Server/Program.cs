using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Log2Server.Models;
using Log2Server.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "LOG2DEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddDbContext<DbLsContext>(oa =>
        {
            var cs = hc.Configuration.GetConnectionString("Main");
            oa.UseMySQL(cs);
        });
        services.AddHostedService<Log2DemoTcpService>();
        services.AddHostedService<Log2DemoUdpService>();
        
    })
    .ConfigureLogging((hc, cl) =>
    {
        cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
        cl.AddConsole();
        cl.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    })
    .UseConsoleLifetime()
    .Build();

host.Run();
