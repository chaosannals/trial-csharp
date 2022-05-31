using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynAstNodeFunction : IDynAstNode
    {
        public string Name { get; set; }
        public DynAstNodeBlock Block { get; set; }

        public string Explain()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name} => {{");
            sb.AppendLine($"{Block.Explain()} }}");
            return sb.ToString();
        }
    }
}
