using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace LogClient;

public class LogDemoClient
{
    private Socket sock;
    private IPEndPoint target;

    public LogDemoClient(IPEndPoint? target = null)
    {
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this.target = target ?? new IPEndPoint(IPAddress.Parse("127.0.0.1"), 33333);
    }

    public void Send(byte[] data)
    {
        sock.SendTo(data, SocketFlags.None, target);
    }
}
