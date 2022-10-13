using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LogServer.Models;
using LogServer.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "LOGDEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddHostedService<LogDemoTcpService>();
        services.AddHostedService<LogDemoUdpService>();
        services.AddDbContext<DbLsContext>(oa =>
        {
            var cs = hc.Configuration.GetConnectionString("Main");
            oa.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 26)));
        });
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

