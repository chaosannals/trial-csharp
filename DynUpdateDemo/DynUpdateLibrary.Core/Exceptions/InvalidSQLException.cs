using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynUpdateLibrary.Core.Exceptions
{
    public class InvalidSQLException : Exception
    {
        public InvalidSQLException(string message) : base(message)
        {

        }
    }
}
