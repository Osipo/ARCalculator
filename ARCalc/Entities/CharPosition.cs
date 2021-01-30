using System;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Entities
{
    public struct CharPosition
    {
        private char _ch;
        private int _pos;

        public CharPosition(char c, int p)
        {
            _ch = c;
            _pos = p;
        }

        public int Pos { get { return _pos; } }
        public char Ch { get { return _ch; } }
    }
}
