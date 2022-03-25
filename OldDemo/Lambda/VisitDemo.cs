      using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Lambda
{
    public class VisitDemo
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public int Age { get; set; }

        public static void OtherScope(Delegate d)
        {
            Console.WriteLine(d.DynamicInvoke());
        }

        public static void VisitRun()
        {
            int age = 100;
            VisitDemo other = new VisitDemo()
            {
                Age = 99
            };
            Expression<Func<VisitDemo, bool>> expr = v => v.Name.Contains("test") && v.Birthday >= DateTime.Now && v.Age < age || age > other.Age;
            Console.WriteLine(expr.Type);
            Console.WriteLine(expr.NodeType);
            Console.WriteLine(expr.Body.NodeType);
            // 判断是值类型后直接取出值，可用作 SQL 里面的字面量。
            var lambda = Expression.Lambda(((expr.Body as BinaryExpression).Right as BinaryExpression).Left as MemberExpression).Compile();
            Console.WriteLine(lambda);
            Console.WriteLine(lambda.DynamicInvoke());
            OtherScope(Expression.Lambda(((expr.Body as BinaryExpression).Right as BinaryExpression).Right as MemberExpression).Compile());
            Console.ReadLine();
        }

    }
}
