using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Log2Server.Services;

public class Log2DemoTcpService : BackgroundService
{
    private int port;
    private ILogger<Log2DemoTcpService> logger;

    public Log2DemoTcpService(IConfiguration configuration, ILogger<Log2DemoTcpService> logger)
    {
        port = configuration.GetValue<int>("Tcp:Port");
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var iep = new IPEndPoint(IPAddress.Any, port);
        sock.NoDelay = true;
        sock.Bind(iep);
        sock.Listen(0);
        
        logger.LogInformation("tcp services start.");

        try
        {
            await Parallel.ForEachAsync(new List<Task>
            {
                AcceptAsync(sock, stoppingToken),
                ReceiveAsync(stoppingToken),
            }, stoppingToken, async (t,_) => await t) ;
        }
        catch (Exception e)
        {
            logger.LogError("tcp error: {0}", e);
        }

        logger.LogInformation("tcp services end.");
    }

    public async Task AcceptAsync(Socket socket, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await socket.AcceptAsync();
            logger.LogInformation("accept: {0}", client.RemoteEndPoint);
        }
    }

    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
        }
    }
}
