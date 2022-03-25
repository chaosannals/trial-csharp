using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TimeDate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i <= 100; i++)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(500);
            }
        }
    }
}
