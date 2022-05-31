using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynInterpreter
    {
        private AppDomain domain;
        private AssemblyName aname;
        private AssemblyBuilder builder;
        private ModuleBuilder mainModuleBuilder;
        private TypeBuilder mainTypeBuider;

        public DynInterpreter()
        {
            //domain = AppDomain.CreateDomain("DynDomain", null, new AppDomainSetup {
            //    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            //});

            //domain.DoCallBack(() =>
            //{
            //    var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("temp"), AssemblyBuilderAccess.Run);
            //    var module = assembly.DefineDynamicModule("DynModule");
            //    var typeBuilder = module.DefineType("MyTempClass", TypeAttributes.Public | TypeAttributes.Serializable);
            //});
            domain = AppDomain.CurrentDomain;
            aname = new AssemblyName("DynAssembly");
            builder = domain.DefineDynamicAssembly(aname, AssemblyBuilderAccess.RunAndSave);
            mainModuleBuilder = builder.DefineDynamicModule("Dyn");
            mainTypeBuider = mainModuleBuilder.DefineType("DynMain");
        }

        public void Interpret()
        {

        }
    }
}
