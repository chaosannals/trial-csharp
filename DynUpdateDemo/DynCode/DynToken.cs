using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public enum DynToken
    {
        EndOfFile = -1,
        
        Identifier = 1,
        Number,
        String,

        KeywordDef,
        KeywordRet,
        KeywordIf,

        SymbolPlus, // + 
        SymbolMinus, // -
        SymbolEqual, // =
        SymbolSemicolon, // ;
        SymbolCurlyLeft, // {
        SymbolCurlyRight, // }
        SymbolPareLeft, // (
        SymbolPareRight, // )
    }
}
