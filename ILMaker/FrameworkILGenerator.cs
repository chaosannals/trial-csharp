using System.Reflection;
using System.Reflection.Emit;

namespace ILMaker
{
    /// <summary>
    /// 跨平台已经废弃了 System.Reflection.Emit
    /// AssemblyBuilder， ILGenerator  等都缺少核心的方法。
    /// </summary>
    internal class FrameworkILGenerator
    {
        public static void DoGenerator()
        {
            Console.WriteLine("begin generate");
            string name = "ILMakerDemo.ILDemoClass";
            string fileName = name + ".dll";

            // 定义程序集
            AssemblyName aname = new AssemblyName(fileName);
            AssemblyBuilder builder = AssemblyBuilder.DefineDynamicAssembly(aname, AssemblyBuilderAccess.RunAndCollect);

            if (aname is null)
            {
                return;
            }

            // 定义模块
            ModuleBuilder mb = builder.DefineDynamicModule(name);

            // 定义类
            TypeBuilder tb = mb.DefineType(name, TypeAttributes.Public);

            // 定义字段
            FieldBuilder fName = tb.DefineField("name", typeof(string), FieldAttributes.Private);
            FieldBuilder fAge = tb.DefineField("age", typeof(int), FieldAttributes.Private);

            // 定义属性 Name
            MethodBuilder nameGet = tb.DefineMethod(
                "get",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(string),
                Type.EmptyTypes
            );
            ILGenerator ilgNameGet = nameGet.GetILGenerator();
            ilgNameGet.Emit(OpCodes.Ldarg_0); // this 入栈
            ilgNameGet.Emit(OpCodes.Ldfld, fName); // 结果入栈
            ilgNameGet.Emit(OpCodes.Ret);
            MethodBuilder nameSet = tb.DefineMethod(
                "set",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { typeof(string) }
            );
            ILGenerator ilgNameSet = nameSet.GetILGenerator();
            ilgNameSet.Emit(OpCodes.Ldarg_0); // this 入栈
            ilgNameSet.Emit(OpCodes.Ldarg_1); // value 入栈
            ilgNameSet.Emit(OpCodes.Stfld, fName); // 字符串赋值
            ilgNameSet.Emit(OpCodes.Ret);
            PropertyBuilder pName = tb.DefineProperty("Name", PropertyAttributes.None, typeof(string), null);
            pName.SetGetMethod(nameGet);
            pName.SetSetMethod(nameSet);

            // 定义属性 Age
            MethodBuilder ageGet = tb.DefineMethod(
                "get",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(int),
                null
            );
            ILGenerator ilgAgeGet = ageGet.GetILGenerator();
            ilgAgeGet.Emit(OpCodes.Ldarg_0);
            ilgAgeGet.Emit(OpCodes.Ldfld, fAge);
            ilgAgeGet.Emit(OpCodes.Ret);
            MethodBuilder ageSet = tb.DefineMethod(
                "set",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { typeof(int) }
            );
            ILGenerator iglAgeSet = ageSet.GetILGenerator();
            iglAgeSet.Emit(OpCodes.Ldarg_0);
            iglAgeSet.Emit(OpCodes.Ldarg_1);
            iglAgeSet.Emit(OpCodes.Stfld, fAge);
            iglAgeSet.Emit(OpCodes.Ret);
            PropertyBuilder pAge = tb.DefineProperty("Age", PropertyAttributes.None, typeof(int), null);
            pAge.SetGetMethod(ageGet);
            pAge.SetSetMethod(ageSet);

            // 构造函数
            ConstructorBuilder cor = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.HasThis,
                new Type[] { typeof(string), typeof(int) }
            );
            ILGenerator iglCor = cor.GetILGenerator();
            iglCor.Emit(OpCodes.Ldarg_0);
            iglCor.Emit(OpCodes.Ldarg_1);
            iglCor.Emit(OpCodes.Stfld, fName);
            iglCor.Emit(OpCodes.Ldarg_0);
            iglCor.Emit(OpCodes.Ldarg_2);
            iglCor.Emit(OpCodes.Stfld, fAge);
            iglCor.Emit(OpCodes.Ret);

            // 定义方法
            MethodInfo? writeLine = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(string), typeof(object), typeof(object) }
            );
            if (writeLine is not null)
            {
                MethodBuilder doSome = tb.DefineMethod(
                    "doSome",
                    MethodAttributes.Public
                );
                ILGenerator ilgDoSome = doSome.GetILGenerator();
                ilgDoSome.Emit(OpCodes.Ldstr, "Name: {0} ,Age: {1}");
                ilgDoSome.Emit(OpCodes.Ldarg_0);
                ilgDoSome.Emit(OpCodes.Call, nameGet);
                ilgDoSome.Emit(OpCodes.Ldarg_0);
                ilgDoSome.Emit(OpCodes.Call, ageGet);
                ilgDoSome.Emit(OpCodes.Box, typeof(int));
                ilgDoSome.Emit(OpCodes.Call, writeLine);
                ilgDoSome.Emit(OpCodes.Ret);
            }

            // 生成 .net 跨平台的都没有了 AssemblyBuilder.Save 方法。
            tb.CreateType();
            Console.WriteLine(builder);
            
            Console.WriteLine("end generate");
        }
    }
}
