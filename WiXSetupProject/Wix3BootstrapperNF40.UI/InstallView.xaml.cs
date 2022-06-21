using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wix3BootstrapperNF40.UI
{
    /// <summary>
    /// InstallView.xaml 的交互逻辑
    /// </summary>
    public partial class InstallView : Window
    {
        public InstallView(InstallViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Closed += (sender, e) =>
            viewModel.CancelCommand.Execute(this);
        }
    }
}
