using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace Resp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 6379))
            {
                using (var stream = client.GetStream())
                {
                    byte[] saveCommand = Encoding.UTF8.GetBytes("SAVE\r\n");
                    stream.Write(saveCommand, 0, saveCommand.Length);
                    byte[] buffer = new byte[1024];
                    int c = stream.Read(buffer, 0, buffer.Length);
                    var result = Encoding.UTF8.GetString(buffer, 0, c);
                    Console.WriteLine(result);
                    /*
                    s.Write(Encoding.UTF8.GetBytes("SAVE\r\n"));
                    byte[] buffer = new byte[1024];
                    int c = s.Read(buffer, 0, buffer.Length);
                    var result = Encoding.UTF8.GetString(buffer, 0, c);
                    if (result.StartsWith("+OK"))
                    {
                        Console.WriteLine("OK");
                    }
                    Console.WriteLine(result);
                    */
                }
            }
        }
    }
}
