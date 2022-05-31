using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynException : Exception
    {
        public DynException(string msg) : base(msg)
        {

        }
    }
}
