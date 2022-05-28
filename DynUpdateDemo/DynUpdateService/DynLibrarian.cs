using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace DynUpdateService
{
    public class DynLibrarian : MarshalByRefObject, IDisposable
    {
        const string DynDomainName = "DynLibraryDomain";
        const string DynWorkerName = "DynUpdateLibrary.DynWorker";

        private object dynWorker = null;

        public static AppDomain NewDomain()
        {
            return AppDomain.CreateDomain(DynDomainName);
        }

        public static DynLibrarian NewInstance(AppDomain dynDomain)
        {
            return dynDomain.CreateInstanceAndUnwrap(
                typeof(DynLibrarian).Assembly.FullName,
                typeof(DynLibrarian).FullName
            ) as DynLibrarian;
        }

        /// <summary>
        /// 调用该方法时为远程调用，已经在 新建的 AppDomain 里面了。
        /// 与主上下文不一致，日志等全局配置失效。
        /// </summary>
        /// <param name="codes"></param>
        public void Start(byte[] codes, string root)
        {
            DynLogger.Init(".s.log", root);
            "DynLibrarian.Start".Log();
            Assembly dynAssembly = AppDomain.CurrentDomain.Load(codes);
            dynWorker = dynAssembly.CreateInstance(DynWorkerName);
            Invoke("Init", root);
            Invoke("Start");
        }

        /// <summary>
        /// 远程调用
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        public void Invoke(string action, params object[] args)
        {
            Type t = dynWorker.GetType();
            MethodInfo mi = t.GetMethod(action);
            mi.Invoke(dynWorker, args);
        }

        public void Dispose()
        {
            if (dynWorker != null)
            {
                Invoke("Stop");
                dynWorker = null;
            }
        }
    }
}
