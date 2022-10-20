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
    private Task? runTask;
    private CancellationTokenSource runCts;

    public TcpClientService(ILogger<TcpClientService> logger)
    {
        this.logger = logger;
        runCts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        try
        {
            runCts.Cancel();
            logger.LogInformation("tcp client dispose.");
        }
        catch (Exception e)
        {
            logger.LogError("tcp client service dispose error. {0}", e);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        runTask = RunAsync(runCts.Token);
        if (runTask.IsCompleted)
        {
            await runTask;
        }
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("tcp client start.");
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
        }
        logger.LogInformation("tcp client end.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (runTask != null)
        {
            try
            {
                runCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(
                    runTask,
                    Task.Delay(Timeout.Infinite, cancellationToken)
                );
            }
        }
        logger.LogInformation("tcp client stop.");
    }
}
