using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopRougamoDemo;

internal class TargetFirst
{
    [Replace(Result = 1234)]
    public static int DoReturnInt()
    {
        Console.WriteLine($"TargetFirst DoReturnInt");
        return 0;
    }

    [Logging]
    public static int SyncFunc(int a)
    {
        Console.WriteLine($"TargetFirst SyncFunc {a}");
        return a + 1;
    }

    [Logging]
    public static int ThrowPublicExceptionFunc()
    {
        throw new NotImplementedException();
    }

    [Logging]
    public static R ThrowExceptionFunc<R>(R i)
    {
        Console.WriteLine($"ThrowExceptionFunc {i}");
        throw new NotImplementedException();
    }

    [LoggingNoTry]
    public static int SyncNoTryFunc(int a)
    {
        Console.WriteLine($"TargetFirst SyncFunc {a}");
        return a + 1;
    }

    [Logging]
    public static async ValueTask<int> AsyncFunc(int a)
    {
        Console.WriteLine($"TargetFirst AsyncFunc {a}");
        await Task.CompletedTask;
        return a + 1;
    }
}
