using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using log4net;
using ILMerging;

[assembly:log4net.Config.XmlConfigurator(Watch = true)]
namespace DynUpdateServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program).FullName);
            log.Info("开始");

            string workdir = AppDomain.CurrentDomain.BaseDirectory;
            string webroot = Path.Combine(workdir, "wwwroot");

            string initpath = Path.Combine(workdir, "init.txt");
            if (File.Exists(initpath))
            {
                string rdir = File.ReadAllText(initpath).Trim();
                log.Info($"rdir: {rdir}");

                log.Info("开始合并");
                DynPacker packer = new DynPacker(Properties.Resources.PackKey);
                packer.Pack(rdir, webroot);
                log.Info("合并完成");

                log.Info("启动服务器");
                DynServer server = new DynServer(webroot);
                server.Start();
            }
            else
            {
                log.Error("init.txt is not exists.");
            }
        }
    }
}
