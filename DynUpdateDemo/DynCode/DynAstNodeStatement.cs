using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynAstNodeStatement : IDynAstNode
    {
        public bool IsReturn { get; set; } = false;
        public DynAstNodeExpression Expression { get; set; }

        public void Effect(DynMachine machine)
        {
            ILGenerator ilg = machine.CurrentMethodBuilder.GetILGenerator();
            if (IsReturn)
            {

                ilg.Emit(OpCodes.Ret);
            }
            else
            {

            }
        }

        public string Explain()
        {
            StringBuilder sb = new StringBuilder();
            if (IsReturn)
            {
                sb.Append("return ");
            }

            sb.AppendLine($"{Expression.Explain()} ;");
            return sb.ToString();
        }
    }
}
