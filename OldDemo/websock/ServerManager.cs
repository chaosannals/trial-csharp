using System.Threading;

namespace WebSock
{
    public class ServerManager
    {
        private Thread thread;

        public ServerManager()
        {
            thread = new Thread(new ThreadStart(Work));
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            thread.Abort();
        }

        private void Work()
        {
            WebSocketServer server = new WebSocketServer(12345);
            server.Start();
        }
    }
}