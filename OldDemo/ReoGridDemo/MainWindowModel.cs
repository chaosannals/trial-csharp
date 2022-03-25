using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ReoGridDemo
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? sheetName;

        public string? SheetName
        {
            get { return sheetName; }
            set
            {
                sheetName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SheetName)));
            }
        }
    }
}
