using System;
using System.Net;
using System.Net.Sockets;

namespace WebSock
{
    public class WebSocketServer
    {
        private Socket socket;

        public WebSocketServer(int port)
        {
            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Start()
        {
            socket.Listen(20);
            socket.BeginAccept(new AsyncCallback(Accept), socket);
        }

        private void Accept(IAsyncResult iar)
        {
            Socket server = iar.AsyncState as Socket;
            Socket client = server.EndAccept(iar);
            // 开是下一个请求。
            server.BeginAccept(new AsyncCallback(Accept), server);
        }
    }
}