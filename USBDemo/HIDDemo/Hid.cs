using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemo;

public static class Hid
{

    [DllImport("hid.dll", EntryPoint = "HidD_GetHidGuid", SetLastError = true)]
    public static extern void GetHidGuid(ref Guid hidGuid);
}
