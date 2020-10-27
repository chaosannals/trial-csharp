using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Trial.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            new TimeTask().Test(); // 测试 Task
            Console.WriteLine("{0:D}", PortManager.FindUsablePort(9000));
        }
    }
}
