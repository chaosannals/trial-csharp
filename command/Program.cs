using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trial.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            var one = Task<TimeSpan>.Run(() =>
            {
                DateTime start = DateTime.Now;
                Thread.Sleep(2500);
                return DateTime.Now.Subtract(start);
            });
            var two = Task<int>.Run(() =>
            {
                DateTime start = DateTime.Now;
                Thread.Sleep(6500);
                return DateTime.Now.Subtract(start);
            });
            Console.WriteLine("{0:N0}", one.Result.TotalMilliseconds);
            Console.WriteLine("{0:N0}", two.Result.TotalMilliseconds);
        }
    }
}
