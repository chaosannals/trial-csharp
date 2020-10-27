using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Trial.Command
{
    public class TimeTask
    {
        private List<int> result;

        public async Task<TimeSpan> Full(int count) {
            return await Task<TimeSpan>.Run(() =>
            {
                DateTime start = DateTime.Now;
                var rand = new Random();
                for (int i = 0; i < count; ++i)
                {
                    lock (result)
                    {
                        result.Add(rand.Next());
                        result.Add(i);
                    }
                }
                return DateTime.Now.Subtract(start);
            });
        }

        public async Task Test() {
            result = new List<int>();
            var one = await Full(1000);
            var two = await Full(10000);
            Console.WriteLine("one time: {0:N0}", one.TotalMilliseconds);
            Console.WriteLine("two time: {0:N0}", two.TotalMilliseconds);
            Console.WriteLine("result count: {0:N0}", result.Count);
        }
    }
}