using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogServer.Services;

public class LogDemoTcpService : IHostedService, IDisposable
{
    private ILogger<LogDemoTcpService> logger;

    public LogDemoTcpService(IConfiguration configuration, ILogger<LogDemoTcpService> logger)
    {
        this.logger = logger;
    }

    public void Dispose()
    {
        
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("tcp services start");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("tcp services stop");
    }
}
