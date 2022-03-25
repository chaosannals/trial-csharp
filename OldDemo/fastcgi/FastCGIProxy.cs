using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FastCGI
{
    class FastCGIProxy
    {
        public FastCGIProxy()
        {

        }

        public async Task Listen(int port = 19000)
        {
            IPAddress address = IPAddress.Parse("0.0.0.0");
            TcpListener listener = new TcpListener(address, port);
            listener.Start();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                
            }
        }
    }
}