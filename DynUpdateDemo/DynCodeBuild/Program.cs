using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DynCodeBuild
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DemoExe();
            DemoDll2();
            DemoDll();
        }

        static void DemoExe()
        {
            var assemblyName = new AssemblyName("Kitty");
            var assemblyBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("KittyModule", "Kitty.exe");
            var typeBuilder = moduleBuilder.DefineType("HelloKittyClass", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod(
                "SayHelloMethod",
                MethodAttributes.Public | MethodAttributes.Static,
                null, null);
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello, Kitty!");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            var helloKittyClassType = typeBuilder.CreateType();
            assemblyBuilder.Save("Kitty.exe");
        }

        static void DemoDll2()
        {
            var assemblyName = new AssemblyName("Kitty");
            var assemblyBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("KittyModule", "Kitty.dll");
            var typeBuilder = moduleBuilder.DefineType("HelloKittyClass", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod(
                "SayHelloMethod",
                MethodAttributes.Public | MethodAttributes.Static,
                null, null);
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello, Kitty!");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            var helloKittyClassType = typeBuilder.CreateType();
            assemblyBuilder.Save("Kitty.dll");
        }

        static void DemoDll()
        {
            var filename = "build.dll"; // 如果要保存的话，DefineDynamicModule 和 Save 名字必须一致。
            var name = new AssemblyName("DynAssembly");
            var assemblyBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Dyn", filename);

            var anyTypeParent = typeof(object);
            var typeBuilder = moduleBuilder.DefineType("DynMain", TypeAttributes.Public, anyTypeParent);
            var kvfb = typeBuilder.DefineField("kind", typeof(int), FieldAttributes.Public);
            var svfb = typeBuilder.DefineField("stringValue", typeof(string), FieldAttributes.Public);
            var nvfb = typeBuilder.DefineField("numberValue", typeof(double), FieldAttributes.Public);
            
            var stringTypeBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[]
            {
                typeof(string)
            });
            var silg = stringTypeBuilder.GetILGenerator();
            silg.Emit(OpCodes.Ldarg_0);
            silg.Emit(OpCodes.Call, anyTypeParent.GetConstructor(Type.EmptyTypes));
            silg.Emit(OpCodes.Ldarg_0);
            silg.Emit(OpCodes.Ldarg_1);
            silg.Emit(OpCodes.Stfld, svfb);
            silg.Emit(OpCodes.Ldarg_0);
            silg.Emit(OpCodes.Ldc_I4, 2);
            silg.Emit(OpCodes.Stfld, kvfb);
            silg.Emit(OpCodes.Ret);

            var numberTypeBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[]
            {
                typeof(double)
            });
            var nilg = numberTypeBuilder.GetILGenerator();
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Call, anyTypeParent.GetConstructor(Type.EmptyTypes));
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Ldarg_1);
            nilg.Emit(OpCodes.Stfld, nvfb);
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Ldc_I4, 1);
            nilg.Emit(OpCodes.Stfld, kvfb);
            nilg.Emit(OpCodes.Ret);

            var t = typeBuilder.CreateType();

            Console.WriteLine(t.FullName);

            assemblyBuilder.Save(filename);

            // Console.ReadKey();
        }
    }
}
