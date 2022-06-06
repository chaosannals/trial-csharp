using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    [Serializable]
    public class DynAstNodeFunctionDefine : DynAstNode
    {
        public string Name { get; set; }
        public List<DynLexeme> Parameters { get; set; }
        public DynAstNodeBlock Block { get; set; }

        public override DynAstType Type => DynAstType.FunctionDefine;

        public override string Explain()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} (");
            sb.Append(string.Join(",", Parameters.Select(i => i.ToString()).ToArray()));
            sb.AppendLine(") => {");
            sb.AppendLine($"{Block.Explain()} }}");
            return sb.ToString();
        }
    }
}
