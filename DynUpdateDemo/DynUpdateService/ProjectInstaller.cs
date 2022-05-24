using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DynUpdateService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void dynServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(dynServiceInstaller.ServiceName))
            {
                sc.Start();
            }
        }

        private void dynServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
