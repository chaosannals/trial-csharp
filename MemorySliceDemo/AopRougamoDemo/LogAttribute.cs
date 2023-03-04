using Rougamo;
using Rougamo.Context;

namespace AopRougamoDemo;

internal class LogAttribute : MoAttribute
{
    /// <summary>
    /// 通过这个修改访问范围，方便直接注解在类上或程序集上。
    /// [assembly: Log] 这种污染感觉有点大。
    /// </summary>
    public override AccessFlags Flags => AccessFlags.Public;

    public override void OnEntry(MethodContext context)
    {
        // 从context对象中能取到包括入参、类实例、方法描述等信息
        Console.WriteLine($"{context.Method.Name}: 方法执行前");
    }

    public override void OnException(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法执行异常", context.Exception);
    }

    public override void OnSuccess(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法执行成功后");
    }

    public override void OnExit(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法退出时，不论方法执行成功还是异常，都会执行");
    }
}
