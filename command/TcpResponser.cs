using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Trial.Command
{
    public class TcpResponser
    {
        public int Port { get; private set; }
        public IPAddress Ip { get; private set; }

        public TcpResponser(int port, IPAddress ip = null)
        {
            Port = port;
            Ip = ip ?? IPAddress.Parse("0:0:0:0");
        }

        public async void Listen()
        {
            TcpListener listener = new TcpListener(Ip, Port);
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
            }
        }
    }
}