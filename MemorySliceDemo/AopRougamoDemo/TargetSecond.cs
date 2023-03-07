using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopRougamoDemo;

[Log]
internal class TargetSecond
{
    public static void SyncPublicFunc(int i)
    {
        Console.WriteLine($"SyncPublicFunc {i}");
        SyncPrivateFunc(++i);
        SyncProtectedFunc(++i);
    }

    public static void ThrowPublicExceptionFunc()
    {
        throw new NotImplementedException();
    }

    private static void SyncPrivateFunc(int i)
    {
        Console.WriteLine($"SyncPrivateFunc {i}");
    }

    protected static void SyncProtectedFunc(int i)
    {
        Console.WriteLine($"SyncPrivateFunc {i}");
    }
}
