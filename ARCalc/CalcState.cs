using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc
{
    public enum CalcState
    {
        ZERO = 0, /* operand is not typed yet. (ZERO-operand) */
        NUM_TYPED = 1, /* digit is typed. */
        DOT_PUSHED = 2, /* '.' symbol typed */
        PAREN_CLOSED = 4, /* ')' typed */
        GOT_RESULT = 5, /* '=' typed */
        ERROR = 6 /* illegal state */
    }
}
