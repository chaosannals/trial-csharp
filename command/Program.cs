using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Trial.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            // new TimeTask().Test().Wait(); // 测试 Task
            // Console.WriteLine("{0:D}", PortManager.FindUsablePort(9000));
            TcpResponser responser = new TcpResponser(12300);
            responser.Listen();
            TcpRequester requester = new TcpRequester(IPAddress.Parse("127.0.0.1"), 12300);
            Console.WriteLine(requester.Send("Hello").Result);
            Console.WriteLine(requester.Send("Hello World").Result);
            for (int i = 0; i <= 100; ++i)
            {
                Console.Write(i);
                Console.Write(" => ");
                Console.WriteLine(requester.Send("Hello World Net Test").Result);
                Thread.Sleep(1000 + i);
            }
        }
    }
}
