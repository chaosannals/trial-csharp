using Rougamo;
using Rougamo.Context;

namespace AopRougamoDemo;

/// <summary>
/// 提供接口实现方式
/// </summary>
internal class LogImplement : IMo
{
    public AccessFlags Flags => AccessFlags.All;

    public void OnEntry(MethodContext context)
    {
        // 从context对象中能取到包括入参、类实例、方法描述等信息
        Console.WriteLine($"{context.Method.Name}: 方法执行前");
    }

    public void OnException(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法执行异常", context.Exception);
    }

    public void OnSuccess(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法执行成功后");
    }

    public void OnExit(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法退出时，不论方法执行成功还是异常，都会执行");
    }
}
