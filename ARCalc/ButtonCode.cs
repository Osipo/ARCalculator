using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc
{
    public enum ButtonCode : int
    {
        ZERO = 0,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8,
        NINE = 9,
        COMMA = 10,

        OP_BEGIN = 11,

        PLUS = 12,
        MINUS = 13,
        MUL = 14,
        DIV = 15,
        OP_PAR = 16,
        CL_PAR = 17,

        OP_END = 18,

        CLEAR = 19,
        SIGN = 20,
        ARROW = 21,
        EQ = 22,
        SPACE = 23,
        BACK = 24,
        FACT = 25,
        FIBO = 26,
        ERR = 27
    }
}
