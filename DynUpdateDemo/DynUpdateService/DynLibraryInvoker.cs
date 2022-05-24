using System;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Threading;

namespace DynUpdateService
{
    public class DynLibraryInvoker
    {
        private Thread invoker;

        public void Attach()
        {
            try
            {
                "Attach".Log();

                invoker = new Thread(() =>
                {
                    while (true) {
                        try
                        {
                            DynUnpacker unpacker = new DynUnpacker(Properties.Resources.UnpackKey);
                            var data = unpacker.Unpack();

                            if (data is null)
                            {
                                "解包失败".Log();
                                Thread.Sleep(10000);
                                continue;
                            }

                            "Init".Log();
                            PrintAssembly();
                            var dm = DynLibrarian.NewDomain();
                            "NewDomain".Log();
                            var lib = DynLibrarian.NewInstance(dm);
                            "NewInstance".Log();
                            PrintAssembly();
                            lib.Start(data);
                            "Start".Log();
                            PrintAssembly();
                            lib.Stop();
                            AppDomain.Unload(dm);
                            "Stop".Log();
                            PrintAssembly();
                            Thread.Sleep(500);
                        }
                        catch (Exception e)
                        {
                            "异常：{0}".Log(e);
                            Thread.Sleep(10000);
                        }
                    }
                    
                });
                invoker.Start();
            }
            catch (Exception e)
            {
                "attach erro {0}".Log(e);
            }
        }

        public void Detach()
        {
            try
            {
                if (invoker != null)
                {
                    invoker.Abort();
                    invoker = null;
                }
            }
            catch (Exception e)
            {
                "detach erro {0}".Log(e);
            }
        }

        public void PrintAssembly()
        {
            "Main {0}:".Log(AppDomain.CurrentDomain.FriendlyName);
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                "Main Assembly: {0}".Log(a.FullName);
            }
        }
    }
}
