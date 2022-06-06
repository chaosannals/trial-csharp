using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace DynCode
{
    [Serializable]
    public class DynAstNodeStatement : DynAstNode
    {
        public bool IsReturn { get; set; } = false;
        public DynAstNodeExpression Expression { get; set; }

        public override DynAstType Type => DynAstType.Statement;

        public override string Explain()
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
