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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports.TemplateEngine;

namespace CrystalUi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReportDocument report;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void onClickOpen(object sender, RoutedEventArgs e)
        {
            if (report == null)
            {

                report = new ReportDocument();

                //var a = new ReportTemplateEngine();
                
                
                // report.Load("");
                //DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                //dt.Rows.Add("第一行");
                //ds.Tables.Add(dt);
                //report.SetDataSource(ds);
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (report != null)
            {
                report.Dispose();
                report = null;
            }
        }
    }
}
