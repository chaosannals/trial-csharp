using System;
using System.Windows.Forms;
using WixSharp;
using WixSharp.Bootstrapper;
using WixSharp.UI.WPF;

namespace WixSharpWpfDemo
{
    internal class Program
    {
        static void Main()
        {
            var project = new ManagedProject("MyProduct",
                              new Dir(@"%ProgramFiles%\My Company\My Product",
                                  new File("Program.cs")));
            project.Language = "zh-CN";

            project.GUID = new Guid("6f330b47-2577-43ad-9095-1861bb258777");

            // project.ManagedUI = ManagedUI.DefaultWpf; // all stock UI dialogs

            //custom set of UI WPF dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add<WixSharpWpfDemo.WelcomeDialog>()
                                            .Add<WixSharpWpfDemo.LicenceDialog>()
                                            .Add<WixSharpWpfDemo.FeaturesDialog>()
                                            .Add<WixSharpWpfDemo.InstallDirDialog>()
                                            .Add<WixSharpWpfDemo.ProgressDialog>()
                                            .Add<WixSharpWpfDemo.ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<WixSharpWpfDemo.MaintenanceTypeDialog>()
                                           .Add<WixSharpWpfDemo.FeaturesDialog>()
                                           .Add<WixSharpWpfDemo.ProgressDialog>()
                                           .Add<WixSharpWpfDemo.ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            string productMsi = project.BuildMsi();

            var bootstrapper = new Bundle("My Product",
                //new PackageGroupRef("NetFx40Web"),
                new MsiPackage(productMsi)
                {
                    DisplayInternalUI = true,// 显示默认 UI
                }
            );

            bootstrapper.Version = new Version("1.0.0.0");
            bootstrapper.UpgradeCode = new Guid("6f330b47-2577-43ad-9095-1861bb25889b");
            bootstrapper.Application = new SilentBootstrapperApplication();

            // 注：以下的名字不填有可能冲突，填要注意不重要冲突。
            bootstrapper.Build("MyProject.exe");
        }
    }
}