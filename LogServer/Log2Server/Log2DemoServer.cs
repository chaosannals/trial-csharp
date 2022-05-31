using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Log2Server;

public class Log2DemoServer
{
    private UdpClient client;
    public Log2DemoServer(int port = 44444)
    {
        client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
    }
}
