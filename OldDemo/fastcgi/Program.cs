using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FastCGI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start !");
            FastCGIProxy proxy = new FastCGIProxy();
            proxy.Listen(19000).Wait();
            Console.WriteLine("End !");
        }
    }
}
