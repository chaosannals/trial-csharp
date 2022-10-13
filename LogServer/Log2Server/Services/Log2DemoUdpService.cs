using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Log2Server.Services;

public class Log2DemoUdpService : BackgroundService
{
    private int port;
    private ILogger<Log2DemoUdpService> logger;

    public Log2DemoUdpService(IConfiguration configuration, ILogger<Log2DemoUdpService> logger)
    {
        port = configuration.GetValue<int>("Udp:Port");
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sock.Bind(new IPEndPoint(IPAddress.Any, port));

        logger.LogInformation("udp bind: {0}", port);

        while (!stoppingToken.IsCancellationRequested)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1024];
            var r = await sock.ReceiveFromAsync(buffer, SocketFlags.None, endPoint);
        }

        logger.LogInformation("udp end: {0}", port);
    }
}
