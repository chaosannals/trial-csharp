using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClrW;

namespace CLRUser
{
    class Program
    {
        static void Main(string[] args)
        {
            ClrWrapper cw = new ClrWrapper();
            for (int i = 0; i <= 10; ++i)
            {
                Console.WriteLine(cw.AddA());
            }
            Console.ReadKey();
        }
    }
}
