using Rougamo;
using Rougamo.Context;
using System.Reflection;

namespace AopRougamoDemo;

/// <summary>
/// 下面方法的代码会被切入，而不是执行构建的 Pass 。
/// </summary>
internal class LoggingAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 从context对象中能取到包括入参、类实例、方法描述等信息
        Console.WriteLine($"{context.Method.Name}: 方法执行前");
    }

    public override void OnException(MethodContext context)
    {
        Console.WriteLine($"{context.Method.Name}: 方法执行异常", context.Exception);
        // 这里把异常处理了，就可以不用 try
        //context.Method;
        var method = context.Method as MethodInfo;
        if (method != null && method.ReturnType.Equals(typeof(int)))
        {
            context.HandledException(this, 123);
        }
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
