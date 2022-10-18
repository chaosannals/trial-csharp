﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Log2Client.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "LOGDEMO2_");
    })
     .ConfigureLogging((hc, cl) =>
     {
         cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
         cl.AddConsole();
     })
     .ConfigureServices((hc, services) =>
     {
         services.AddHostedService<TcpClientService>();
         services.AddHostedService<UdpClientService>();
     })
    .UseConsoleLifetime()
    .Build();

host.Run();