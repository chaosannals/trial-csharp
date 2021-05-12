
using System;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Spider
{
    public class IpAddressInfo
    {
        public static void GetIps()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Console.WriteLine(ni.Name);
                foreach (UnicastIPAddressInformation uipi in ni.GetIPProperties().UnicastAddresses)
                {
                    if (uipi.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(uipi.Address);
                    }
                }
            }
        }
    }
}