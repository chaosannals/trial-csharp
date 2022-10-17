using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogClient.Services;

public class TcpClientService : IHostedService, IDisposable
{
    private ILogger<TcpClientService> logger;

    public TcpClientService(ILogger<TcpClientService> logger)
    {
        this.logger = logger;
    }

    public void Dispose()
    {
        logger.LogInformation("tcp client dispose.");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("tcp client start.");
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        logger.LogInformation("tcp client stop.");
    }
}
