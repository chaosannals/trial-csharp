using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogClient.Services;

public class UdpClientService : IHostedService, IDisposable
{
    private Socket sock;
    private IPEndPoint target;
    private ILogger<UdpClientService> logger;
    private Task? runTask;
    private CancellationTokenSource runCts;

    public UdpClientService(IConfiguration configuration, ILogger<UdpClientService> logger)
    {
        var host = configuration.GetValue<string>("UdpServer:Host");
        var port = configuration.GetValue<int>("UdpServer:Port");
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        target = new IPEndPoint(IPAddress.Parse(host), port);
        this.logger = logger;
        runCts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        runCts.Cancel();
        sock.Dispose();
        logger.LogInformation("udp client dispose.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("udp client start: {0}", target);
        runTask = RunAsync(runCts.Token);
        if (runTask.IsCompleted)
        {
            return runTask;
        }
        return Task.CompletedTask;
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
        if (runTask is null)
        {
            return;
        }

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
            logger.LogInformation("udp client stop.");
        }
    }
}
