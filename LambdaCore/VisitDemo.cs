using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LambdaCore
{
    public class VisitDemo
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public static void Run()
        {
            Expression<Func<VisitDemo, bool>> expr = i => i.Name.StartsWith("AAA");
            Console.WriteLine(expr.Type);
        }
    }
}
