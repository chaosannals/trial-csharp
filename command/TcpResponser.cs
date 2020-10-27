using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
            Ip = ip ?? IPAddress.Parse("0.0.0.0");
        }

        public async void Listen()
        {
            TcpListener listener = new TcpListener(Ip, Port);
            listener.Start();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[8096];
                while (client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        int count = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string info = string.Format("read count: {0:N0}", count);
                        byte[] data = Encoding.UTF8.GetBytes(info);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
        }
    }
}