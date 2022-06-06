using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    [Serializable]
    public class DynAstNodeExpression : DynAstNode
    {
        public DynAstNodeOperand Left { get; set; }
        public DynLexeme Operation { get; set; }
        public DynAstNodeExpression Right { get; set; }

        public override DynAstType Type => DynAstType.Expression;

        public override string Explain()
        {
            return $"{Left?.Explain()} {Operation} {Right?.Explain()}";
        }
    }
}
