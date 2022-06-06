using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    [Serializable]
    public class DynAstNodeOperand : DynAstNode
    {
        public DynLexeme Lexeme { get; set; }

        public override DynAstType Type => DynAstType.Operand;

        public override string Explain()
        {
            return Lexeme.ToString();
        }
    }
}
