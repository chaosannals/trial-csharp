using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemo;

public struct SP_DEVICE_INTERFACE_DATA_32
{
    public int cbSize;
    public Guid interfaceClassGuid;
    public int flags;
    public int reserved;
}

public struct SP_DEVICE_INTERFACE_DATA_64
{
    public int cbSize;
    public Guid interfaceClassGuid;
    public int flags;
    public long reserved;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public struct SP_DEVICE_INTERFACE_DETAIL_DATA_32
{
    public int cbSize;
    public short devicePath; // 原 C 结构是一个占位的数组，把这个结构套上后这个字段就是字符串数组。因为对齐 sizeof 是 6，故 C# 下用这个占位，它不是 short
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public struct SP_DEVICE_INTERFACE_DETAIL_DATA_64
{
    public int cbSize;
    public int devicePath; // 原 C 结构是一个占位的数组，把这个结构套上后这个字段就是字符串数组。因为对齐 sizeof 是 8，故 C# 下用这个占位，它不是 int
}

[StructLayout(LayoutKind.Sequential)]
public class SP_DEVINFO_DATA
{
    public int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
    public Guid classGuid = Guid.Empty; // temp
    public int devInst = 0; // dumy
    public int reserved = 0;
}

public enum DIGCF
{
    DIGCF_DEFAULT = 0x00000001, // only valid with DIGCF_DEVICEINTERFACE
    DIGCF_PRESENT = 0x00000002,
    DIGCF_ALLCLASSES = 0x00000004,
    DIGCF_PROFILE = 0x00000008,
    DIGCF_DEVICEINTERFACE = 0x00000010
}

public static class SetupApi
{
    [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassDevsW", SetLastError = true)]
    public static extern IntPtr DiGetClassDevs(ref Guid hidGuid, IntPtr enumerator, IntPtr hwndParent, DIGCF flags);

    [DllImport("setupapi.dll", EntryPoint = "SetupDiDestroyDeviceInfoList", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DiDestroyDeviceInfoList(IntPtr deviceInfoSet);


    [DllImport("setupapi.dll", EntryPoint = "SetupDiEnumDeviceInterfaces", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DiEnumDeviceInterfaces64(
        IntPtr deviceInfoSet,
        IntPtr deviceInfoData,
        ref Guid interfaceClassGuid,
        uint memberIndex,
        ref SP_DEVICE_INTERFACE_DATA_64 deviceInterfaceData
    );

    [DllImport("setupapi.dll", EntryPoint = "SetupDiGetDeviceInterfaceDetailW", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DiGetDeviceInterfaceDetail64(
        IntPtr deviceInfoSet,
        ref SP_DEVICE_INTERFACE_DATA_64 deviceInterfaceData,
        IntPtr deviceInterfaceDataDetailData,
        int deviceInterfaceDetailDataSize,
        ref int requiredSize,
        SP_DEVINFO_DATA deviceInfoData
    );
}
