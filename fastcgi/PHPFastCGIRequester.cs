using System.Collections.Generic;
using System.Net.Sockets;
using FastCGI;

namespace PHP
{
    /// <summary>
    /// 请求者
    /// </summary>
    public class PHPFastCGIRequester
    {
        public TcpClient Source { get; private set; }

        public PHPFastCGIRequester(TcpClient source)
        {
            Source = source;
        }

        public IEnumerable<FastCGIMessage> Transfer(TcpClient target)
        {
            byte[] buffer = new byte[8192];
            NetworkStream requester = Source.GetStream();
            NetworkStream responser = target.GetStream();
            FastCGIParser sfcp = new FastCGIParser();
            FastCGIParser tfcp = new FastCGIParser();
            while (true)
            {
                if (requester.DataAvailable)
                {
                    int count = requester.Read(buffer, 0, buffer.Length);
                    responser.Write(buffer, 0, count);
                    sfcp.Gain(buffer, count);
                    while (true)
                    {
                        FastCGIMessage m = sfcp.Pop();
                        if (m == null) break;
                        yield return m;
                    }
                }
                if (responser.DataAvailable)
                {
                    int count = responser.Read(buffer, 0, buffer.Length);
                    requester.Write(buffer, 0, count);
                    tfcp.Gain(buffer, count);
                    while (true)
                    {
                        FastCGIMessage m = tfcp.Pop();
                        if (m == null) break;
                        yield return m;
                    }
                }
                yield return null;
            }
        }
    }
}
