using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AopRougamoDemo;

/// <summary> 
/// 不支持泛型 ReplaceAttribute<T>
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ReplaceAttribute : MoAttribute
{
    //public Type ResultType { get; init; } = null!;
    public object Result { get; init; } = null!;

    public override void OnSuccess(MethodContext context)
    {
        Console.WriteLine($"ReplaceAttribute {context.Method.Name}: 方法执行成功后");

        var method = context.Method as MethodInfo;
        var resultType = Result.GetType();
        if (method != null && method.ReturnType.Equals(resultType))
        //if (method != null && method.ReturnType.Equals(ResultType))
        {
            context.HandledException(this, Result);
        }
    }
}
