using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynMachine : MarshalByRefObject, IDisposable
    {
        
        public string FileName { get; private set; }
        public AssemblyName Name { get; private set; }
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder mainModuleBuilder;
        private TypeBuilder mainTypeBuider;

        public HashSet<string> GlobalVariables { get; private set; } = new HashSet<string>();

        public string LastError { get; set; }

        private Type anyType;

        public DynMachine()
        {
            FileName = "dyn.dll";
            Name = new AssemblyName("DynAssembly");
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(Name, AssemblyBuilderAccess.RunAndSave);
            mainModuleBuilder = assemblyBuilder.DefineDynamicModule("Dyn", FileName);
            mainTypeBuider = mainModuleBuilder.DefineType("DynMain", TypeAttributes.Public);
            anyType = CreateDynAnyType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type CreateDynAnyType()
        {
            var anyTypeParent = typeof(object);
            var anyTypeBuilder = mainModuleBuilder.DefineType("DynAny", TypeAttributes.Public, anyTypeParent);
            var kfb = anyTypeBuilder.DefineField("kind", typeof(int), FieldAttributes.Public);
            var svfb = anyTypeBuilder.DefineField("stringValue", typeof(string), FieldAttributes.Public);
            var nvfb = anyTypeBuilder.DefineField("numberValue", typeof(double), FieldAttributes.Public);

            var stringTypeBuilder = anyTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[]
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
            silg.Emit(OpCodes.Stfld, kfb);
            silg.Emit(OpCodes.Ret);

            var numberTypeBuilder = anyTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[]
            {
                typeof(double)
            });
            var nilg = numberTypeBuilder.GetILGenerator();
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Call, anyTypeParent.GetConstructor(new Type[] { }));
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Ldarg_1);
            nilg.Emit(OpCodes.Stfld, nvfb);
            nilg.Emit(OpCodes.Ldarg_0);
            nilg.Emit(OpCodes.Ldc_I4, 1);
            nilg.Emit(OpCodes.Stfld, kfb);
            nilg.Emit(OpCodes.Ret);

            //var addMethodBuilder = anyTypeBuilder.DefineMethod(
            //    "add_operator",
            //    MethodAttributes.Public | MethodAttributes.Static,
            //    anyTypeBuilder,
            //    new Type[] { anyTypeBuilder, anyTypeBuilder }
            //);
            //var ailg = addMethodBuilder.GetILGenerator();
            //ailg.Emit(OpCodes.Ldarg_0);
            //// ailg.Emit(OpCodes.Ldfld, kfb);
            //ailg.Emit(OpCodes.Ldfld, nvfb);
            //ailg.Emit(OpCodes.Ldarg_1);
            //ailg.Emit(OpCodes.Ldfld, nvfb);
            //ailg.Emit(OpCodes.Add);
            //ailg.Emit(OpCodes.Newobj, numberTypeBuilder);
            //ailg.Emit(OpCodes.Ret);

            return anyTypeBuilder.CreateType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public void Start(DynAstNodeRoot root)
        {
            try
            {
                foreach (var n in root.Statements)
                {
                    switch (n.Type)
                    {
                        case DynAstType.FunctionDefine:
                            CreateFunction(n as DynAstNodeFunctionDefine);
                            break;
                    }
                }
                mainTypeBuider.CreateType();
            }
            catch (DynException e)
            {
                LastError = $"{e.Message} {e.StackTrace}";
            }
        }

        public void Save()
        {
            assemblyBuilder.Save(FileName);
        }

        public void Run()
        {
            try
            {
                var mi = mainTypeBuider.GetMethod("main");
                var r = mi.Invoke(null, null);
            }
            catch (DynException e)
            {
                LastError = $"{e.Message} {e.StackTrace}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fd"></param>
        public void CreateFunction(DynAstNodeFunctionDefine fd)
        {
            var methodBuilder = mainTypeBuider.DefineMethod(
                fd.Name,
                MethodAttributes.Public | MethodAttributes.Static,
                anyType,
                fd.Parameters.Select(i => anyType).ToArray()
            );
            var milg = methodBuilder.GetILGenerator();
            Dictionary<string, LocalBuilder> methodScope = new Dictionary<string, LocalBuilder>();
            Dictionary<string, int> args = new Dictionary<string, int>();
            for (int pi = 0; pi < fd.Parameters.Count; ++pi)
            {
                args.Add(fd.Parameters[pi].Identifier, pi);
            }
            foreach (var s in fd.Block.Statements)
            {
                switch(s.Type)
                {
                    case DynAstType.Statement:
                        {
                            var ss = s as DynAstNodeStatement;
                            EmitExpression(ss.Expression, milg, methodScope, args);
                            if (ss.IsReturn)
                            {
                                var r = milg.DeclareLocal(anyType);
                                var nle = milg.DefineLabel();
                                milg.Emit(OpCodes.Dup);
                                milg.Emit(OpCodes.Isinst, typeof(int));
                                milg.Emit(OpCodes.Ldnull);
                                milg.Emit(OpCodes.Cgt);
                                milg.Emit(OpCodes.Brfalse, nle);
                                milg.Emit(OpCodes.Newobj, anyType.GetConstructor(new Type[] { typeof(int) }));
                                milg.Emit(OpCodes.Stloc, r);
                                milg.Emit(OpCodes.Ldloc, r);
                                milg.Emit(OpCodes.Ret);
                                milg.MarkLabel(nle);

                                var sle = milg.DefineLabel();
                                milg.Emit(OpCodes.Dup);
                                milg.Emit(OpCodes.Isinst, typeof(string));
                                milg.Emit(OpCodes.Ldnull);
                                milg.Emit(OpCodes.Cgt);
                                milg.Emit(OpCodes.Brfalse,sle);
                                milg.Emit(OpCodes.Newobj, anyType.GetConstructor(new Type[] { typeof(string) }));
                                milg.Emit(OpCodes.Stloc, r);
                                milg.Emit(OpCodes.Ldloc, r);
                                milg.Emit(OpCodes.Ret);
                                milg.MarkLabel(sle);

                                //var e = milg.DeclareLocal(typeof(DynException));
                                //milg.Emit(OpCodes.Ldstr, $"语法错误，未支持的类型。");
                                //milg.Emit(OpCodes.Newobj);
                                //milg.Emit(OpCodes.Stloc, e);
                                //milg.Emit(OpCodes.Ldloc, e);
                                //milg.ThrowException(typeof(DynException));

                                milg.Emit(OpCodes.Ldnull);
                                milg.Emit(OpCodes.Ret);
                            }
                            break;
                        }
                    default:
                        throw new DynException($"语法错误，函数内部出现非语句，{s.Type}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ilg"></param>
        /// <param name="scope"></param>
        /// <param name="args"></param>
        /// <exception cref="DynException"></exception>
        public void EmitExpression(DynAstNodeExpression e, ILGenerator ilg, Dictionary<string, LocalBuilder> scope, Dictionary<string, int> args)
        {
            if (e.Operation is null)
            {
                EmitOperand(e.Left, ilg, scope, args);
                return;
            }

            switch (e.Operation.Token)
            {
                case DynToken.SymbolEqual:
                    {
                        if (e.Left.Lexeme.Token != DynToken.Identifier)
                        {
                            throw new DynException($"语法错误，赋值操作只可作用于左值。");
                        }
                        // Right
                        EmitExpression(e.Right, ilg, scope, args);

                        if (GlobalVariables.Contains(e.Left.Lexeme.Identifier))
                        {
                            ilg.Emit(OpCodes.Stfld, e.Left.Lexeme.Identifier);
                            break;
                        }
                        if (!scope.ContainsKey(e.Left.Lexeme.Identifier))
                        {
                            LocalBuilder lbt = ilg.DeclareLocal(anyType);
                            scope.Add(e.Left.Lexeme.Identifier, lbt);
                        }
                        LocalBuilder lb = scope[e.Left.Lexeme.Identifier];
                        ilg.Emit(OpCodes.Stloc, lb.LocalIndex);
                        break;
                    }
                case DynToken.SymbolPlus:
                    {
                        // Left
                        EmitOperand(e.Left, ilg, scope, args);

                        // Right
                        EmitExpression(e.Right, ilg, scope, args);

                        ilg.Emit(OpCodes.Add);
                        //ilg.Emit(OpCodes.Call, anyType.GetMethod("add_operator"));
                        break;
                    }
                case DynToken.SymbolMinus:
                    {
                        // Left
                        EmitOperand(e.Left, ilg, scope, args);

                        // Right
                        EmitExpression(e.Right, ilg, scope, args);

                        ilg.Emit(OpCodes.Sub);
                        break;
                    }
                case DynToken.SymbolStar:
                    {
                        // Left
                        EmitOperand(e.Left, ilg, scope, args);

                        // Right
                        EmitExpression(e.Right, ilg, scope, args);

                        ilg.Emit(OpCodes.Mul);
                        break;
                    }
                default:
                    throw new DynException($"语法错误，未知的表达式 {e.Operation}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="ilg"></param>
        /// <param name="scope"></param>
        /// <param name="args"></param>
        /// <exception cref="DynException"></exception>
        public void EmitOperand(DynAstNodeOperand op, ILGenerator ilg, Dictionary<string, LocalBuilder> scope, Dictionary<string, int> args)
        {
            switch (op.Lexeme.Token)
            {
                case DynToken.Number:
                    ilg.Emit(OpCodes.Ldc_R8, op.Lexeme.Number.Value);
                    ilg.Emit(OpCodes.Newobj, anyType.GetConstructor(new Type[] { typeof(double) } ));
                    return;
                case DynToken.Identifier:
                    if (scope.ContainsKey(op.Lexeme.Identifier))
                    {
                        LocalBuilder lb = scope[op.Lexeme.Identifier];
                        ilg.Emit(OpCodes.Ldloc, lb.LocalIndex);
                    }
                    else if (args.ContainsKey(op.Lexeme.Identifier))
                    {
                        ilg.Emit(OpCodes.Ldarg, args[op.Lexeme.Identifier]);
                    }
                    else if (GlobalVariables.Contains(op.Lexeme.Identifier))
                    {
                        ilg.Emit(OpCodes.Ldfld, op.Lexeme.Identifier);
                    }
                    else
                    {
                        throw new DynException($"语法错误，未知变量 {op.Lexeme}");
                    }
                    return;
            }
            throw new DynException($"语法错误，未知的操作数 {op.Lexeme}");
        }

        public void Dispose()
        {
            
        }
    }
}
