using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp.Server;
using System.Threading;
using log4net;

namespace DynUpdateServer
{
    internal class DynServer
    {
        private static Dictionary<string, string> MIME = new Dictionary<string, string>() {
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/x-javascript" },
            { ".json", "application/json" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".ico", "image/x-icon" },
            { ".ttf", "application/octet-stream" },
            { ".woff", "application/octet-stream" },
            { ".map", "application/json" },
            { ".dll", "application/octet-stream" },
            { ".dup", "application/octet-stream" },
            { ".dyn", "application/octet-stream" },
        };

        private ILog log;
        private HttpServer server;
        public string WebRoot { get; private set; }
        public string WebIndexPath { get; private set; }

        public DynServer(string webroot, int port=33333, bool secure=false)
        {
            log = LogManager.GetLogger(typeof(DynServer));
            server = new HttpServer(IPAddress.Any, port, secure);
            WebRoot = webroot;
            WebIndexPath = Path.Combine(webroot, "index.html");
            if (secure)
            {
                server.SslConfiguration.ServerCertificate = new X509Certificate2(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chhd.pfx"),
                    "",
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.Exportable
                );
                // 因为 XP 的 360 浏览器不支持 TLS1.2，所以使用 TLS1.0
                // 因为 .net framework 3.5 TLS 版本太低。
                // 使用强制转换 新版本 TLS 1.2 的枚举值。
                server.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls | (SslProtocols)0xC00;
            }
            //server.AddWebSocketService<WsDemoDispatcher>("/wsdemo");
            server.OnGet += new EventHandler<HttpRequestEventArgs>(OuptutExamples);
        }

        public void Start()
        {
            server.Start();
            while (server.IsListening)
            {
                Thread.Yield();
            }
        }

        private void OuptutExamples(object sender, HttpRequestEventArgs e)
        {
            string location = e.Request.Url.AbsolutePath.Trim('/').Replace('/', '\\');
            string filePath = Path.Combine(WebRoot, location);
            string suffix = Path.GetExtension(filePath).ToLower();
            if (!File.Exists(filePath) || !MIME.ContainsKey(suffix))
            {
                filePath = WebIndexPath;
                suffix = ".html";
            }
            byte[] contents = File.ReadAllBytes(filePath);
            e.Response.ContentType = MIME[suffix];
            e.Response.ContentLength64 = contents.Length;
            e.Response.StatusCode = 200;
            e.Response.Close(contents, true);
            log.InfoFormat("[{1}]: {0}", filePath, e.Response.ContentType);
        }
    }
}
