using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Lambda
{
    public static class SimpleExtends
    {
        public static bool SimpleCall(this DateTime self)
        {
            return self.Year > 2000;
        }
    }


    public class SimpleDemo
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public static void SimpleRun()
        {
            Expression<Func<SimpleDemo, bool>> lambda = (i) => i.Name != "123" && i.Birthday <= System.DateTime.Today && i.Birthday <= DateTime.Now || i.Birthday.SimpleCall();
            Console.WriteLine(lambda);
            Console.WriteLine(lambda.Parameters[0]);
            foreach (var p in lambda.Parameters)
            {
                Console.WriteLine(p);
            }
            Console.WriteLine(lambda.Body);
            var regex = new Regex($"({lambda.Parameters[0]}\\.)(\\w+)");
            var sql = regex.Replace(lambda.Body.ToString(), "$2");
            sql = sql.Replace("DateTime.Today", "CONVERT(DATE, GETDATE())");
            sql = sql.Replace("DateTime.Now", "GETDATE()");
            Console.WriteLine(sql);
            Console.ReadLine();
        }
    }
}
