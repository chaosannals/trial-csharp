using System;
using System.Threading;
using System.Collections.Generic;

namespace Spider
{
    public class Generator
    {
        public IEnumerable<int> GetEnumerable(int i)
        {
            while (i < 20)
            {
                Thread.Sleep(1000);
                yield return ++i;
            }
        }

        public static void DoTest()
        {
            Generator g = new Generator();
            IEnumerable<int> e = g.GetEnumerable(10);
            IEnumerator<int> er = e.GetEnumerator();
            foreach (int i in e)
            {
                Console.WriteLine(i);
            }
            while (er.MoveNext()) {
                Console.WriteLine(er.Current);
            }
        }
    }
}