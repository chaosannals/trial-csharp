using Rougamo;
using Rougamo.Context;

namespace AopRougamoDemo;

/// <summary>
/// 即使不 override 还是会切入基础版本。
/// 例如： try 也会出现在 字节码 里。
/// </summary>
internal class LoggingNoTryAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 从context对象中能取到包括入参、类实例、方法描述等信息
        Console.WriteLine($"notry: {context.Method.Name}: 方法执行前");
    }
}
