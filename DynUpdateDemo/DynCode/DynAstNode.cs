using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    [Serializable]
    public abstract class DynAstNode
    {
        public abstract string Explain();
        public abstract DynAstType Type { get; }
    }
}
