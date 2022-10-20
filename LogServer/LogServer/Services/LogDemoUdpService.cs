using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LogServer.Services;

public class LogDemoUdpService : IHostedService, IDisposable
{
    private Socket sock;
    private IPEndPoint bind;
    private ILogger<LogDemoUdpService> logger;
    private Task? runTask;
    private CancellationTokenSource runCts;

    public LogDemoUdpService(IConfiguration configuration, ILogger<LogDemoUdpService> logger)
    {
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        bind = new IPEndPoint(IPAddress.Any, configuration.GetValue<int>("Udp:Port"));
        this.logger = logger;
        runCts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        try
        {
            runCts.Cancel();
            sock?.Dispose();
            logger.LogInformation("udp service dispose.");
        }
        catch (Exception e)
        {
            logger.LogError("udp service dispose error: {0}", e);
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
        sock.Bind(bind);
        byte[] buffer = new byte[512];
        logger.LogInformation("udp services start.");

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
            //    EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //    int pl = sock.ReceiveFrom(buffer, ref sender);
            //    byte[] data = buffer.Take(pl).ToArray();
            //    logger.LogInformation("receive: {0} ", data.Length);
        }
        logger.LogInformation("udp services end.");
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
        logger.LogInformation("udp services end.");
    }
}
