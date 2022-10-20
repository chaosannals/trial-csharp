using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LogServer.Services;

public class LogDemoTcpService : IHostedService, IDisposable
{
    private ILogger<LogDemoTcpService> logger;
    private Task? runTask;
    private CancellationTokenSource runCts;

    public LogDemoTcpService(IConfiguration configuration, ILogger<LogDemoTcpService> logger)
    {
        this.logger = logger;
        runCts = new CancellationTokenSource();
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
        logger.LogInformation("tcp service start.");
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
        }
        logger.LogInformation("tcp service end.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (runTask!= null)
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
        logger.LogInformation("tcp service stop.");
    }

    public void Dispose()
    {
        try
        {
            runCts.Cancel();
            logger.LogInformation("tcp service dispose.");
        }
        catch(Exception e)
        {
            logger.LogError("tcp service dispose error: {0}", e);
        }
    }
}
