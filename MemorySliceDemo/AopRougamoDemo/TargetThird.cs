using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopRougamoDemo;

internal class TargetThird : ITarget
{
    public async Task DoAsync(string message)
    {
        await Task.Yield();
        Console.WriteLine($"Do Async {message}");
    }

    public void DoSync(string message)
    {
        Console.WriteLine($"Do Sync {message}");
    }

    public void NoByInterface()
    {
        Console.WriteLine($"NoByInterface");
    }

    [DefaultValue]
    public void DoSome(string? test=null, int i=123)
    {
        if (test != null) Console.WriteLine("default value working");
        else Console.WriteLine("default value not working");
    }
}
