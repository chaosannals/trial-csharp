using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
// using System.Windows.Forms;
using Microsoft.Win32;
using unvell.ReoGrid;

namespace ReoGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowModel Model { get; private set; }
        public MainWindow()
        {
            Model = new MainWindowModel();
            InitializeComponent();
            DataContext = this;
        }

        private void OnClickLoadExcel(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Supported file format(*.xlsx;*.rgf;*.xml)|*.xlsx;*.rgf;*.xml|Excel 2007 Document(*.xlsx)|*.xlsx|ReoGrid Format(*.rgf;*.xml)|*.rgf;*.xml";
            var r = ofd.ShowDialog();
            if (r.HasValue && r.Value)
            {
                Model.SheetName = ofd.FileName;
                ExcelGrid.Load(ofd.FileName);
            }
        }
        private void OnClickSaveExcel(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            var r = sfd.ShowDialog();
            if (r.HasValue && r.Value)
            {
                Model.SheetName = sfd.FileName;
                ExcelGrid.Save(sfd.FileName);
            }
        }
    }
}
