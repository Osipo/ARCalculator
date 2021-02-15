using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Exceptions
{
    public class MaximumCapacityException : Exception
    {
        public MaximumCapacityException()
        {

        }
        public MaximumCapacityException(string msg) : base(msg)
        {

        }

        public MaximumCapacityException(string msg, Exception inner) : base(msg, inner)
        {

        }
    }
}
