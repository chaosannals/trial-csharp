using System;
using System.Threading;

namespace WebSock
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerManager manager = new ServerManager();
            manager.Start();
            Console.WriteLine("Hello World!");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
