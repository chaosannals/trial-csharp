using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    [Serializable]
    public class DynAstNodeRoot : DynAstNode
    {
        public List<DynAstNode> Statements { get; set; }

        public override DynAstType Type => DynAstType.Root;

        public override string Explain()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DynAstNode node in Statements)
            {
                sb.Append(node.Explain());
            }
            return sb.ToString();
        }
    }
}
