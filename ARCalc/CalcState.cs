using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc
{
    public enum CalcState
    {
        ZERO = 0,
        NUM_TYPED = 1,
        DOT_PUSHED = 2,
        OP_PUSHED = 3,
        PAREN_CLOSED = 4,
        GOT_RESULT = 5,
        ERROR = 6
    }
}
