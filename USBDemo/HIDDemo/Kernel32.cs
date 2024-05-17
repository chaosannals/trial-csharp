using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemo;

public class Kernel32
{
    [DllImport("kernel32.dll")]
    public static extern int GetLastError();
}
