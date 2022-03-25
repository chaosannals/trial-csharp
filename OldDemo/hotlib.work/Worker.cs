using System;
using HotLib.Core;

namespace HotLib.Work
{
    public class Worker : IWorkable
    {
        public void Work()
        {
            Console.WriteLine("Worker.Work");
        }
    }
}
