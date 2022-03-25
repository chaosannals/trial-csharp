using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<string> Send(string content)
        {
            var stream = client.GetStream();
            var data = Encoding.UTF8.GetBytes(content);
            await stream.WriteAsync(data, 0, data.Length);
            byte[] buffer = new byte[8096];
            int count = await stream.ReadAsync(buffer, 0, buffer.Length);
            byte[] result = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, buffer);
            return Encoding.Unicode.GetString(result);
        }
    }
}