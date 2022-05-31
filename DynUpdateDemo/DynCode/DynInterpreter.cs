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
        private string name;
        private AppDomainSetup setup;

        public DynInterpreter(string name = "DynDomain")
        {
            setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            };
        }

        public void Interpret(DynAstNodeRoot root)
        {
            AppDomain domain = AppDomain.CreateDomain(name, null, setup);
            try
            {
                DynMachine machine = domain.CreateInstanceAndUnwrap(
                    typeof(DynMachine).Assembly.FullName,
                    typeof(DynMachine).FullName
                ) as DynMachine;
                root.Effect(machine);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }
    }
}
