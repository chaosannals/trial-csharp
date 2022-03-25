using System;
using System.Text;
using Gtk;

namespace GtkUi
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var window = new Window("Gtk Window");
            window.WindowPosition = WindowPosition.Center;
            window.SetDefaultSize(800, 600);
            window.Resizable = false;
            window.DeleteEvent += (s, e) =>
            {
                Application.Quit();
            };
            window.ShowAll();
            var builder = new Builder();
            builder.AddFromString(Encoding.UTF8.GetString(Properties.Resources.one));
            var w = builder.GetObject("window") as Window;
            w.ShowAll();
            Application.Run();
        }
    }
}
