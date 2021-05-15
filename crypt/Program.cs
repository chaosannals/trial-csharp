using System;

namespace Crypt
{
    public class TestData
    {
        public int a { get; set; }
        public string b { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var key = "b6fa6f5xxg7b219es50ac039eff61dea";
            var a = new TestData();
            a.a = 12346;
            a.b = "aaaaaaabbbbbbcccccccccccccccccccccc";
            var e = Aes256.Encrypt(key, a);
            Console.WriteLine(e);
            var d = Aes256.Decrypt<TestData>(key, e);
            Console.WriteLine(d.a);
            Console.WriteLine(d.b);
        }
    }
}
