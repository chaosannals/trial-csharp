using System;
using System.Net;
using System.Net.Sockets;

namespace FastCGI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IPAddress address = IPAddress.Parse("0.0.0.0");
            TcpListener listener = new TcpListener(address, 19000);
            listener.BeginAcceptTcpClient(ar =>
            {
                TcpListener owner = ar.AsyncState as TcpListener;
                TcpClient source = owner.EndAcceptTcpClient(ar);
                source.SendTimeout = 300000;
                source.ReceiveTimeout = 300000;
            }, listener);
        }
    }
}
