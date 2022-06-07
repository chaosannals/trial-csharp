using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Lokad.ILPack;

namespace DynCodeGen;

public static class DemoGen
{
    public static void GenExe()
    {
        AssemblyName assemblyName = new AssemblyName("DynAssembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("KittyModule");
        var typeBuilder = moduleBuilder.DefineType("HelloKittyClass", TypeAttributes.Public);
        var methodBuilder = typeBuilder.DefineMethod(
            "SayHelloMethod",
            MethodAttributes.Public | MethodAttributes.Static,
            null, null);
        var il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldstr, "Hello, Kitty!");
        il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!);
        il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine")!);
        il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);
        var helloKittyClassType = typeBuilder.CreateType();
        // 没有设置 EntryPoint 的方法，
        // 导出 靠 ILPack 第三方库
        // isuse 说 .net 7.0 可能加入 导出和设置 EntryPoint 

        var generator = new AssemblyGenerator();
        // var codes = generator.GenerateAssemblyBytes(assemblyBuilder);
        generator.GenerateAssembly(assemblyBuilder, "GenKitty.exe");
    }
}
