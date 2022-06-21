using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Wix3BootstrapperNF40.UI
{
    public class CustomBootstrapperApplication : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; set; }
        static public DiaViewModel Model { get; private set; }
        static public InstallView View { get; private set; }

        protected override void Run()
        {
            Model = new DiaViewModel(this);
            Dispatcher = Dispatcher.CurrentDispatcher;
            var model = new BootstrapperApplicationModel(this);
            var viewModel = new InstallViewModel(model);
            View = new InstallView(viewModel);
            model.SetWindowHandle(View);
            this.Engine.Detect();
            View.Show();
            Dispatcher.Run();
            this.Engine.Quit(model.FinalResult);
        }
    }
}
