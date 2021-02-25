using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Exceptions
{
    public class IllegalNumberException : Exception
    {
        public IllegalNumberException() { }

        public IllegalNumberException(string msg) : base(msg)
        {

        }
        public IllegalNumberException(string msg, Exception inner) : base(msg, inner)
        {

        }
    }
}
