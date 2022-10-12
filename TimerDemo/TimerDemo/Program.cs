using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Timers;

namespace TimerDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("on do some:" + DateTime.Now.ToString());
            try
            {
                var Url = "http://127.0.0.1:12345";
                var contents = new List<string> { "123123", "321123" };
                byte[] buffer = Encoding.UTF8.GetBytes(string.Join("", contents.ToArray()) + "\r\n");
                WebRequest request = WebRequest.Create(Url);
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.ContentLength = buffer.Length;
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(buffer, 0, buffer.Length);
                }
                WebResponse response = request.GetResponse();
                response.Close();
            }
            catch { }
            Console.WriteLine("on do some:" + DateTime.Now.ToString());
            Console.ReadKey();
        }

        static void Main2(string[] args)
        {
            object lockObject = new { };

            var Ticker = new Timer();
            Ticker.Elapsed += (sender, a) =>
            {
                try {
                    lock(lockObject)
                    {
                        Console.WriteLine("on do some:" + DateTime.Now.ToString());
                        //System.Threading.Thread.Sleep(5000);

                        var Url = "http://127.0.0.1:12345";
                        var contents = new List<string> { "123123", "321123" };
                        byte[] buffer = Encoding.UTF8.GetBytes(string.Join("", contents.ToArray()) + "\r\n");
                        WebRequest request = WebRequest.Create(Url);
                        request.Timeout = 5000;
                        request.Method = "POST";
                        request.ContentType = "text/plain";
                        request.ContentLength = buffer.Length;
                        using (Stream writer = request.GetRequestStream())
                        {
                            writer.Write(buffer, 0, buffer.Length);
                        }
                        WebResponse response = request.GetResponse();
                        response.Close();
                    }
                } catch { }
            };
            Ticker.Interval = 2000;
            Ticker.Start();

            Console.ReadKey();
        }
    }
}
