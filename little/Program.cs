using System;
using System.Reflection;

namespace Little
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(PropertyInfo p in typeof(AType).GetProperties()) {
                Console.WriteLine(p.Name);
            }
            foreach(FieldInfo f in typeof(AType).GetFields()) {
                Console.WriteLine(f.Name);
            }
        }
    }
}
