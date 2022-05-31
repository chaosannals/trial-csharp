using System.Text;
using LogClient;

var client = new LogDemoClient();

while (true)
{
    Thread.Sleep(1000);
    client.Send(Encoding.UTF8.GetBytes("aaa"));
}