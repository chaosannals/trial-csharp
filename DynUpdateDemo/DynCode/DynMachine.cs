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
        
        public AssemblyName Name { get; private set; }
        public AssemblyBuilder Builder { get; private set; }
        public ModuleBuilder MainModuleBuilder { get; private set; }
        public TypeBuilder MainTypeBuider { get; private set; }
        public MethodBuilder CurrentMethodBuilder { get; private set; } = null;

        public int Scope { get; set; }

        public DynMachine()
        {
            Name = new AssemblyName("DynAssembly");
            Builder = AppDomain.CurrentDomain.DefineDynamicAssembly(Name, AssemblyBuilderAccess.RunAndSave);
            MainModuleBuilder = Builder.DefineDynamicModule("Dyn");
            MainTypeBuider = MainModuleBuilder.DefineType("DynMain", TypeAttributes.Public);
        }

        public void CreateFunction(string name)
        {
            CurrentMethodBuilder = MainTypeBuider.DefineMethod(
                name,
                MethodAttributes.Public | MethodAttributes.Static,
                typeof(object),
                Type.EmptyTypes
           );
        }

        public void Dispose()
        {
            
        }
    }
}
