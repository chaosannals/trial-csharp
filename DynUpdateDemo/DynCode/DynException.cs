using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    //[Serializable]
    public class DynException : Exception
    {
        public DynException(): base(){}
        public DynException(string msg) : base(msg)
        {

        }
    }
}
