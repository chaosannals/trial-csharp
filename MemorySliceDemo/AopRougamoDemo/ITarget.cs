using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopRougamoDemo;

/// <summary>
/// 通过接口方式，使得实现接口的方法被 织入。
/// 真吊诡，即使不是通过这个接口定义的方法也会被织入。（即，实现该接口，就相当于类上套了等价的属性）
/// 所以效果和 属性 一样，而且写法还麻烦。。。
/// </summary>
internal interface ITarget : IRougamo<LogImplement>
{
    void DoSync(string message);
    Task DoAsync(string message);
}
