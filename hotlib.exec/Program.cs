using System;
using System.Threading;

namespace HotLib.Exec
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                HotEventQueue.Run();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Start");
            while (true)
            {
                Thread.Sleep(2000);
            }
        }
    }
}
