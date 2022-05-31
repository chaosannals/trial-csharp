using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace LogServer;

public class LogDemoServer : IDisposable
{
    private Socket sock;
    private IPEndPoint bind;

    public LogDemoServer(int port = 33333)
    {
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        bind = new IPEndPoint(IPAddress.Any, port);
    }

    public void Dispose()
    {
        if (sock != null)
        {
            sock.Dispose();
        }
    }

    public void Serve()
    {
        sock.Bind(bind);
        byte[] buffer = new byte[512];
        while (true )
        {
            EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            int pl = sock.ReceiveFrom(buffer, ref sender);
            byte[] data = buffer.Take(pl).ToArray();
            // Debug.WriteLine("receive: {0} ", data.Length);
            Console.WriteLine("receive: {0} ", data.Length);
        }
    }
}
