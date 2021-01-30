using ARCalc.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using ARCalc.Entities;

namespace ARCalc.Computors
{
    public class Evaluator
    {
        
        public string CanParse(IEnumerable<string> tokens)
        {
            string op1 = null, op2 = null;
            LinkedStack<string> result = new LinkedStack<string>();
            foreach(string cur in tokens)
            {
                if (RPNParser.IsOperator(cur))
                {
                    op2 = result.Top();
                    result.Pop();
                    op1 = result.Top();
                    result.Pop();
                    string r = ApplyOperation(op1, op2, cur);
                    if (r == "Err")
                        throw new InvalidOperationException("Eval");
                    result.Push(r);
                }
                else /* It is an operand. */
                {
                    result.Push(cur);
                }
            }
            return result.Top();
        }

        /// <summary>
        /// Execute arithmetic operation with operands op1 and op2
        /// </summary>
        /// <param name="op1">first operand</param>
        /// <param name="op2">second operand</param>
        /// <param name="operator">arithmetic operation</param>
        /// <returns>A result of arithmetic operation with bigNumbers op1 and op2.</returns>
        private string ApplyOperation(string op1, string op2, string @operator)
        {
            op1 = StringNumber.RemoveSpaces(op1);
            op2 = StringNumber.RemoveSpaces(op2);
            op1 = StringNumber.RemoveUnnecessaryZeros(op1);
            op2 = StringNumber.RemoveUnnecessaryZeros(op2);
            
            string norm_result = null;
            if ((norm_result = ProcessNeutralOperands(op1, op2, @operator)) != null)
                return norm_result;

            int p1 = StringNumber.GetPrecisionNumber(op1);
            int p2 = StringNumber.GetPrecisionNumber(op2);
            int prec = (p1 > p2) ? p1 : p2;
            BigInteger n1 = BigInteger.Parse(StringNumber.UpPowerOfNumber(op1, prec));
            BigInteger n2 = BigInteger.Parse(StringNumber.UpPowerOfNumber(op2, prec));
            BigInteger? result = @operator switch
            {
                "+" => BigInteger.Add(n1, n2),
                "*" => BigInteger.Multiply(n1, n2),
                "-" => BigInteger.Subtract(n1, n2),
                "/" => null,
                _ => null
            };
            if (result.HasValue == false && @operator == "/")
            {
                return StringNumber.DivideRationalNumbers(n1, n2);
            }
            else if(result.HasValue == false)
                return "Err";
            prec = @operator switch
            {
                "*" => prec + prec,
                "+" => prec,
                "-" => prec,
                _ => 0
            };
            norm_result = StringNumber.DownPowerOfNumber(result.ToString(), prec);
            return StringNumber.RemoveUnnecessaryZeros(norm_result);
        }

        /// <summary>
        /// Process some expressions (1 + 0 or 0 * 25, 258 * 1, and so on)
        /// Also handle nonsense ( 1 / 0 or 0 / 0)
        /// </summary>
        /// <param name="op1">first operand</param>
        /// <param name="op2">second operand</param>
        /// <param name="operator">arithmetic operation</param>
        /// <returns>A result of arithmetic operation with bigNumbers op1 and op2.</returns>
        private string ProcessNeutralOperands(string op1, string op2, string @operator)
        {
            return @operator switch
            {
                "+" when op1 == "0" && op2 != "0" => op2,
                "+" when op1 != "0" && op2 == "0" => op1,
                "+" when op1 == "0" && op2 == "0" => "0",
                "-" when op1 == "0" && op2 != "0" && op2[0] != '-' => "-" + op2,
                "-" when op1 == "0" && op2 != "0" => op2[1..],
                "-" when op1 != "0" && op2 == "0" => op1,
                "-" when op1 == "0" && op2 == "0" => "0",
                "*" when op1 == "0" || op2 == "0" => "0",
                "*" when op1 == "1" =>  op2,
                "*" when op2 == "1" =>  op1,
                "/" when op1 == "0" && op2 != "0" => "0",
                "/" when op2 == "0" => "Err",
                "/" when op2 == "1" => op1,
                _ => null
            };
        }
    }
}
