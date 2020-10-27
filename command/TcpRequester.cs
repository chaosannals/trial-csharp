using System.Net;
using System.Net.Sockets;

namespace Trial.Command
{
    public class TcpRequester
    {
        public int Port { get; private set; }
        public IPAddress Ip { get; private set; }

        private TcpClient client;

        public TcpRequester(IPAddress ip, int port)
        {
            Ip = ip;
            Port = port;
            client = new TcpClient(ip.ToString(), port);
        }
    }
}