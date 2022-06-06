using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    [Serializable]
    public enum DynAstType
    {
        Block,
        Expression,
        FunctionDefine,
        Operand,
        Root,
        Statement,
    }
}
