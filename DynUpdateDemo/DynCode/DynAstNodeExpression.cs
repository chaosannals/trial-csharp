using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynAstNodeExpression : IDynAstNode
    {
        public DynAstNodeOperand Left { get; set; }
        public DynToken Operation { get; set; }
        public DynAstNodeExpression Right { get; set; }

        public void Effect(DynMachine machine)
        {
            var ilg = machine.CurrentMethodBuilder.GetILGenerator();
            Left.Effect(machine);
            Right.Effect(machine);
            switch (Operation)
            {
                case DynToken.SymbolPlus:
                    ilg.Emit(OpCodes.Add);
                    break;
                case DynToken.SymbolMinus:
                    ilg.Emit(OpCodes.Sub);
                    break;
            }
        }

        public string Explain()
        {
            return $"{Left?.Explain()} {Operation} {Right?.Explain()}";
        }
    }
}
