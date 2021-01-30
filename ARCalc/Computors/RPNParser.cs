using System;
using System.Collections.Generic;
using System.Text;
using ARCalc.Structures;
namespace ARCalc.Computors
{
    public class RPNParser : IRPNParser
    {
        public static bool IsOperator(String t)
        {
            return t switch
            {
                "+" => true,
                "-" => true,
                "*" => true,
                "/" => true,
                "(" => true,
                ")" => true,
                _ => false
            };
        }
        private int GetPriority(String t)
        {
            return t switch
            {
                "*" => 2,
                "/" => 2,
                "+" => 1,
                "-" => 1,
                "(" => 0,
                ")" => 0,
                _ => -1
            };
        }

        public LinkedStack<String> GetInput(IEnumerable<String> s)
        {
            LinkedStack<String> ops = new LinkedStack<String>();
            LinkedStack<String> rpn = new LinkedStack<String>();
            foreach(String tok in s)
            {
                if (tok == "(")
                {
                    ops.Push(tok);
                }
                else if (tok == ")")
                {
                    while (!ops.IsEmpty() && ops.Top() != "(")
                    {
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Pop();
                }
                else if (!IsOperator(tok))/* Operand */
                {
                    rpn.Push(tok);
                }
                else if (IsOperator(tok))
                {
                    while (!ops.IsEmpty() && IsOperator(ops.Top()) && GetPriority(tok) <= GetPriority(ops.Top()))
                    {
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Push(tok);
                }
            }
            while (!ops.IsEmpty())
            {
                rpn.Push(ops.Top());
                ops.Pop();
            }
            LinkedStack<String> result = new LinkedStack<String>();
            while (!rpn.IsEmpty())
            {
                result.Push(rpn.Top());
                rpn.Pop();
            }
            rpn = null;
            ops = null;
            return result;
        }
    }
}
