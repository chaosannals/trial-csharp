using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemo;

public enum HID_RETURN
{
    SUCCESS = 0,
    NO_DEVICE_CONECTED,
    DEVICE_NOT_FIND,
    DEVICE_OPENED,
    WRITE_FAILD,
    READ_FAILD

}

public static class Hid
{

    [DllImport("hid.dll", EntryPoint = "HidD_GetHidGuid", SetLastError = true)]
    public static extern void GetHidGuid(ref Guid hidGuid);
}
