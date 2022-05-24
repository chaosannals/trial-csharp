using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DynUpdateService
{
    public partial class DynService : ServiceBase
    {
        private DynLibraryInvoker invoker;

        public DynService()
        {
            "服务初始化".Log();
            InitializeComponent();
            invoker = new DynLibraryInvoker();
        }

        protected override void OnStart(string[] args)
        {
            "服务启动".Log();
            invoker.Attach();
        }

        protected override void OnStop()
        {
            invoker.Detach();
            "服务关闭".Log();
        }
    }
}
