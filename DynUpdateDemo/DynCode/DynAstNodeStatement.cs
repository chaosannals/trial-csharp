using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynAstNodeStatement : IDynAstNode
    {
        public bool IsReturn { get; set; } = false;
        public DynAstNodeExpression Expression { get; set; }

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
