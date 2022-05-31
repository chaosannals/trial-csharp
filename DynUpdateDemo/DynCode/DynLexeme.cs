using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynLexeme
    {
        public DynToken Token { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public double? Number { get; set; }
        public string Identifier { get; set; }

        public override string ToString()
        {
            return $"{Token} [{Row},{Column}] {Number} {Identifier}";
        }
    }
}
