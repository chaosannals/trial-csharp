using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Net;

namespace DynUpdateServiceWorker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 下载
            string target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DynTarget.dll");
            if (!File.Exists(target))
            {
                string url = "http://127.0.0.1:33333/DynTarget.dll";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, target);
                }
            }

            var codes = File.ReadAllBytes(target);
            
        }
    }
}
