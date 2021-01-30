using ARCalc.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Computors
{
    public interface IRPNParser
    {
        public LinkedStack<String> GetInput(IEnumerable<String> s);
    }
}
