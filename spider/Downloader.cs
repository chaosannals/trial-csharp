using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.ComponentModel;

namespace Spider
{
    public class Downloader
    {
        private static bool again = true;
        public static void Download()
        {
            Console.WriteLine("Start Download!");
            Uri url = new Uri("https://windows.php.net/downloads/releases/php-8.0.5-nts-Win32-vs16-x64.zip");
            WebClient client = new WebClient();
            client.Headers.Set("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36");
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Downloaded);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Changed);
            client.DownloadFileAsync(url, "php-8.0.5-nts-Win32-vs16-x64.zip");
            while(again) {
                Thread.Sleep(2000);
            }
        }

        public static void Downloaded(object sender, AsyncCompletedEventArgs e)
        {
            again = false;
            Console.WriteLine("Downloaded");
        }

        public static void Changed(object sender, DownloadProgressChangedEventArgs e) {
            Console.WriteLine(e.ProgressPercentage);
        }
    }
}