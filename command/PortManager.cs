using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;

namespace Trial.Command
{
    public class PortManager
    {
        public static int FindUsablePort(int start=0)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = ipProperties.GetActiveTcpListeners();
            int[] ports = tcpEndPoints.Select(p => p.Port).Where(p => p > start).ToArray();
            Array.Sort(ports);
            for (int i = 0; i < ports.Length; ++i)
            {
                int port = ports[i];
                int result = port + 1;
                if (result < ports[i + 1])
                {
                    return result;
                }
            }
            return 0;
        }
    }
}