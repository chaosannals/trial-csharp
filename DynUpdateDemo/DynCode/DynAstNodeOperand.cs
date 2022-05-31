using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynAstNodeOperand : IDynAstNode
    {
        public DynLexeme Lexeme { get; set; }

        public void Effect(DynMachine machine)
        {
            var ilg = machine.CurrentMethodBuilder.GetILGenerator();
            switch (Lexeme.Token)
            {
                case DynToken.Number:
                    //ilg.Emit()
                    break;
                case DynToken.Identifier:
                    break;
            }
        }

        public string Explain()
        {
            return Lexeme.ToString();
        }
    }
}
