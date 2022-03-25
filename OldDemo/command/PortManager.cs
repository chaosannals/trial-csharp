using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;

namespace Trial.Command
{
    public class PortManager
    {
        public static int FindUsablePort(int start = 0)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = ipProperties.GetActiveTcpListeners();
            int[] ports = tcpEndPoints.Select(p => p.Port).Where(p => p >= start).ToArray();
            Array.Sort(ports);
            int result = start;
            int i = 0;
            while (i < ports.Length)
            {
                if (result == ports[i])
                {
                    ++result;
                    ++i;
                }
                else
                {
                    return result;
                }
            }
            return 0;
        }
    }
}