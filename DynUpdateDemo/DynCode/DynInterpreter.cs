using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

namespace DynCode
{
    public class DynInterpreter
    {
        private string name;
        private AppDomainSetup setup;

        public DynInterpreter(string name = "DynDomain")
        {
            this.name = name;
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
                machine.Start(root);
                machine.Save();
                Console.WriteLine("save : {0}", machine.LastError);
                machine.Run();
                Console.WriteLine("run : {0}", machine.LastError);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }
    }
}
