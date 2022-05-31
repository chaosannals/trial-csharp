using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynAstNodeExpression : IDynAstNode
    {
        public DynAstNodeOperand Left { get; set; }
        public DynToken Operation { get; set; }
        public DynAstNodeExpression Right { get; set; }

        public string Explain()
        {
            return $"{Left?.Explain()} {Operation} {Right?.Explain()}";
        }
    }
}
