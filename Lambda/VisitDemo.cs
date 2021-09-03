using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Lambda
{
    public class VisitDemo
    {
        public string Name { get; set; }
        public DateTime Age { get; set; }

        public static void VisitRun()
        {
            Expression<Func<VisitDemo, bool>> expr = v => v.Name.Contains("test") && v.Age >= DateTime.Now;
        }

    }
}
