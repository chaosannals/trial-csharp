using Rougamo;
using Rougamo.Context;

namespace AopRougamoDemo;

internal class DefaultValueAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        context.RewriteArguments = true;

        // 判断参数类型最好通过下面ParameterInfo来判断，而不要通过context.Arguments[i].GetType()
        // 因为context.Arguments[i]可能为null
        var parameters = context.Method.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(string) && context.Arguments[i] == null)
            {
                context.Arguments[i] = string.Empty;
            }
        }
    }
}
