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
            List<int> result = new List<int>();
            var one = Task<TimeSpan>.Run(() =>
            {
                DateTime start = DateTime.Now;
                var rand = new Random();
                for (int i = 0; i < 1000; ++i)
                {
                    lock (result)
                    {
                        // result.Add(rand.Next());
                        result.Add(i);
                    }
                    Thread.Yield();
                }
                return DateTime.Now.Subtract(start);
            });
            var two = Task<int>.Run(() =>
            {
                DateTime start = DateTime.Now;
                var rand = new Random();
                for (int i = 0; i < 1000; ++i)
                {
                    lock (result)
                    {
                        // result.Add(rand.Next());
                        result.Add(i);
                    }
                    Thread.Yield();
                }
                return DateTime.Now.Subtract(start);
            });
            Console.WriteLine("{0:N0}", one.Result.TotalMilliseconds);
            Console.WriteLine("{0:N0}", two.Result.TotalMilliseconds);
            // foreach(int i in result) {
            //     Console.WriteLine("{0:N0}", i);
            // }
            Console.WriteLine("{0:N0}", one.Result.TotalMilliseconds);
            Console.WriteLine("{0:N0}", two.Result.TotalMilliseconds);
            Console.WriteLine("{0:D}", PortManager.FindUsablePort(9000));
        }
    }
}
