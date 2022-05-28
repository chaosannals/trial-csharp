using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DynUpdateService
{
    public static class DynLogger
    {
        private static bool able = true;
        private static Thread logger;
        private static List<string> contents = new List<string>();
        public static string Folder { get; private set; }
        public static string Suffix { get; private set; }

        public static void Init(string suffix=".log", string root=null)
        {
            Suffix = suffix;
            Folder = Path.Combine(root ?? AppDomain.CurrentDomain.BaseDirectory, "log");
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
            logger = new Thread(() =>
            {
                while(able)
                {
                    lock (contents)
                    {
                        try { Write(); } catch { }
                    }
                    Thread.Sleep(1);
                }
            });
            logger.Start();
        }

        public static void Quit()
        {
            able = false;
            logger.Join();
            lock (contents)
            {
                Write();
            }
        }

        public static void Log(this string content, params object[] args)
        {
            string text = string.Format(
                "[{0:S}] - {1:S}\r\n",
                DateTime.Now.ToString("F"),
                string.Format(content, args)
            );
            lock (contents)
            {
                contents.Add(text);
            }
        }

        private static void Write()
        {
            if (contents.Count == 0) return;

            string date = DateTime.Now.ToString("yyyyMMdd");
            string path = Path.Combine(Folder, string.Format("{0:S}{1:S}", date, Suffix));

            // 大于 2M 的文件先搬移
            FileInfo info = new FileInfo(path);
            if (info.Exists && info.Length > 2000000)
            {
                string time = DateTime.Now.ToString("HHmmss");
                info.MoveTo(Path.Combine(Folder, string.Format("{0:S}-{1:S}{2:S}", date, time, Suffix)));
            }

            // 写入日志
            using (FileStream stream = File.Open(path, FileMode.OpenOrCreate | FileMode.Append))
            {
                foreach (string text in contents)
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    stream.Write(data, 0, data.Length);
                }
                contents.Clear();
            }
        }
    }
}
