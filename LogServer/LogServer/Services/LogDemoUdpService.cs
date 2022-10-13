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

    public LogDemoUdpService(IConfiguration configuration, ILogger<LogDemoUdpService> logger)
    {
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        bind = new IPEndPoint(IPAddress.Any, configuration.GetValue<int>("Udp:Port"));
        this.logger = logger;
    }

    public void Dispose()
    {
        sock?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        sock.Bind(bind);
        byte[] buffer = new byte[512];
        logger.LogInformation("udp services start.");
        //while (true)
        //{
        //    EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        //    int pl = sock.ReceiveFrom(buffer, ref sender);
        //    byte[] data = buffer.Take(pl).ToArray();
        //    logger.LogInformation("receive: {0} ", data.Length);
        //}
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("udp services end.");
    }
}
