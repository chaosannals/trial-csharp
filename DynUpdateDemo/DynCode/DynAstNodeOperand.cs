using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynAstNodeOperand : IDynAstNode
    {
        public DynLexeme Lexeme { get; set; }

        public string Explain()
        {
            return Lexeme.ToString();
        }
    }
}
