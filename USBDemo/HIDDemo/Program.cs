using HIDDemo;
using System.Runtime.InteropServices;

const int MAX_USB_DEVICES = 64;

List<string> GetHidDeviceList()
{
    Guid hUsb = Guid.Empty;

    Hid.GetHidGuid(ref hUsb);

    Console.WriteLine(hUsb);

    var result = new List<string>();
    IntPtr hidInfoSet = SetupApi.DiGetClassDevs(ref hUsb, IntPtr.Zero, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);

    if (hidInfoSet != IntPtr.Zero)
    {
        Console.WriteLine("LIST: {0}", Kernel32.GetLastError());
        for (uint i = 0; i < MAX_USB_DEVICES; ++i)
        {
            Console.Write("USB:");
            Console.Write(i);
            var interfaceInfo = new SP_DEVICE_INTERFACE_DATA_64();
            interfaceInfo.cbSize = Marshal.SizeOf(interfaceInfo);

            if (SetupApi.DiEnumDeviceInterfaces64(hidInfoSet, IntPtr.Zero, ref hUsb, i, ref interfaceInfo))
            {
                int buffSize = 0;
                SetupApi.DiGetDeviceInterfaceDetail64(hidInfoSet, ref interfaceInfo, IntPtr.Zero, buffSize, ref buffSize, null);

                Console.Write(" buffSize: ");
                Console.Write(buffSize);

                IntPtr pDetail = Marshal.AllocHGlobal(buffSize);
                if (IntPtr.Size == 8)
                {
                    Marshal.WriteInt32(pDetail, Marshal.SizeOf<SP_DEVICE_INTERFACE_DETAIL_DATA_64>());
                }
                else
                {
                    Marshal.WriteInt32(pDetail, Marshal.SizeOf<SP_DEVICE_INTERFACE_DETAIL_DATA_32>());
                }
                
                if (SetupApi.DiGetDeviceInterfaceDetail64(hidInfoSet, ref interfaceInfo, pDetail, buffSize, ref buffSize, null))
                {
                    var devicePath = Marshal.PtrToStringUni(pDetail + 4);
                    Console.Write(" DEVICE_PATH: ");
                    Console.Write(devicePath);
                    result.Add(devicePath);
                }
                else
                {
                    Console.Write(" ERROR: ");
                    Console.Write(Kernel32.GetLastError());
                }

                Marshal.FreeHGlobal(pDetail);
            }
            Console.WriteLine();
        }
    }

    return result;
}

Console.WriteLine("START");
GetHidDeviceList();
Console.ReadKey();
