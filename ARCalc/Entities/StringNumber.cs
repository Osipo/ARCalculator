using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using ARCalc.Structures;

namespace ARCalc.Entities
{
    public class StringNumber
    {


        /// <summary>
        /// Returns a precision of number bigNum
        /// </summary>
        /// <param name="bigNum">Rational number with certant precision.</param>
        /// <returns>A precision of bigNum. If bigNum is integer, then it returns 0. Value is positive integer.</returns>
        public static int GetPrecisionNumber(string bigNum)
        {
            int idx = bigNum.IndexOf('.');
            if (idx == -1)
                return 0;
            idx = idx + 1;
            return bigNum[idx..^0].Length;
        }

        /// <summary>
        /// Remove space symbol from string bigNum
        /// </summary>
        /// <param name="bigNum">String from what space symbols are being deleted.</param>
        /// <returns>String without space symbols.</returns>
        public static string RemoveSpaces(string bigNum)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < bigNum.Length; i++)
            {
                if (bigNum[i] != ' ')
                    sb.Append(bigNum[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Remove unnecessary zeroes from bigNum. If bigNum is real number (with dot)
        /// zeros are being removed from right side.
        /// Otherwise zeroes are being removed from left side in case of integer number.
        /// </summary>
        /// <param name="bigNum">rational number with certant precision.</param>
        /// <returns>String form of rational number without excees zeroes.</returns>
        public static string RemoveUnnecessaryZeros(string bigNum)
        {
            int l = bigNum.Length - 1;
            int idx = bigNum.IndexOf('.');
            if (idx == -1)
            {
                l = 0;
                while (l < bigNum.Length && bigNum[l] == '0')
                    l++;
                if (l == bigNum.Length)
                    return "0";
                return bigNum[l..];
            }
            else
            {
                while (l >= 0 && bigNum[l] == '0' && l != idx)
                    l--;
                if (l > idx)
                    l += 1;
                return bigNum[0..l];
            }
        }

        /// <summary>
        /// Multiplies bigNum on ten to the power of p (10^p).
        /// </summary>
        /// <param name="bigNum">Rational number with certant precision.</param>
        /// <param name="p">Power. This parameter must be strong positive!</param>
        /// <returns>String form of bigNum which is product of bigNum * 10 ^ p</returns>
        public static string UpPowerOfNumber(string bigNum, int p)
        {
            if (p < 0)
                throw new ArgumentOutOfRangeException($"Illegal value of parameter {nameof(p)}. Value must be >= than 0");
            if (p == 0)
                return bigNum;
            int z_idx = FindFirstNonZeroDigit(bigNum);
            if (z_idx == -1)
                return "0";
            int idx = bigNum.IndexOf('.');
            string zeros = "";
            if(idx == -1) /* integer number, just add p zeros to it */
            {
                return bigNum + new string('0', p);
            }
            if(bigNum[0] != '0') /* a rational number with non-zero integer part [1.23 or 12.34] */
            {
                if(idx + p >= bigNum.Length) /* dot is right further than number => delete dot and append zeros */
                {
                    zeros = new string('0', (idx + p) - (bigNum.Length - 1));
                    return bigNum.Remove(idx, 1) + zeros;
                }
                else if(idx + p == bigNum.Length - 1) /* dot is exactly after number => delete dot */
                {
                    return bigNum.Remove(idx, 1);
                }
                else
                {
                    int lidx = idx + 1;
                    int ridx = idx + p + 1;

                    return bigNum[0..idx] + bigNum[lidx..ridx] + "." + bigNum[ridx..];
                }
            }
            if(z_idx - p == idx) /* move til dot + 1 => 0.01 [2] => 1, 0.0123 [2] => 1.23 */
            {
                int z_idx2 = z_idx + 1;
                if (z_idx2 == bigNum.Length)
                    return bigNum[z_idx..];
                return bigNum[z_idx] + "." + bigNum[z_idx2..];
            }
            else if(z_idx - p < idx)/* move over dot + 1  => DONE: FIX ERROR! */
            {
                int left_zeros = z_idx - idx - 1;   /* zeros which are left from first non-zero digit */
                idx = z_idx; /* start number from non-zero digit */
                
                int l = bigNum[idx..].Length;/* significant digits */
                
                int right_zeros = p - left_zeros - l; /* zeros after significant digits */

                //0.00123 [4] => 12.3
                if (right_zeros >= 0)
                    return bigNum[idx..] + new string('0', right_zeros);
                else { //0.00213 [4] => 2.13 
                    int per_idx = bigNum.Length + right_zeros + 1;/* we must move [rigt_zeros] digits after the period */
                    if (per_idx == bigNum.Length)
                        return bigNum[idx..];
                    return bigNum[idx..per_idx] + "." + bigNum[per_idx..];
                }
            }
            else
            {
                //0.0021 [1] => 0.021
                int left_zeros = z_idx - idx - 1; /* zeros from left side of the first non-zero digit */
                int right_zeros = left_zeros - p;/* zeros between '0.' and first non-zero digit */
                return "0." + new string('0', right_zeros) + bigNum[z_idx..];
            }
        }


        /// <summary>
        /// Multiplies bigNum on ten to the power of - p (10 ^ - p).
        /// </summary>
        /// <param name="bigNum">Rational number with certant precision.</param>
        /// <param name="p">Power. This parameter must be strong negative!</param>
        /// <returns>String form of bigNum which is product of bigNum * 10 ^ ( - p)</returns>
        public static string DownPowerOfNumber(string bigNum, int p)
        {
            if (p < 0)
                throw new ArgumentOutOfRangeException($"Illegal value of parameter {nameof(p)}. Value must be >= than 0");
            if (p == 0)
                return bigNum;
            int z_idx = FindFirstNonZeroDigit(bigNum);
            if (z_idx == -1)
                return "0";
            int idx = bigNum.IndexOf('.');
            if (idx == -1 && p < bigNum.Length)/* integer number with p less than number length. */
            {
                idx = bigNum.Length - p;
                return bigNum[0..idx] + "." + bigNum.Substring(idx);
            }
            else if (idx == -1 && p == bigNum.Length)/* integer number with p that is exactly power of it => just add '0.' to number */
            {
                return "0." + bigNum;
            }
            else if (idx == -1 && p > bigNum.Length)/* absolute value of power p is more than power of number */
            {
                idx = p - bigNum.Length;
                return "0." + new string('0', idx) + bigNum;
            }
            else /* process real number */
                return bigNum;
        }

        /// <summary>
        /// Divide two integer numbers by school-degrade algorithm.
        /// </summary>
        /// <param name="op1">Integer number</param>
        /// <param name="op2">Integer number</param>
        /// <returns>Rational number with certant precision.</returns>
        public static string DivideRationalNumbers(BigInteger n1, BigInteger n2)
        {
            BigInteger rem = BigInteger.Zero;
            string total;
            string r;
            if (n1 < n2)
            {
                total = "0.";
                rem = BigInteger.Parse(n1.ToString() + "0");
            }
            else
            {
                BigInteger div = BigInteger.DivRem(n1, n2, out rem);
                if (rem.Equals(BigInteger.Zero))
                    return div.ToString();

                rem = BigInteger.Parse(rem.ToString() + "0");
                total = div.ToString() + ".";
            }
            LinkedStack<string> dividents = new LinkedStack<string>();
            do
            {
                r = rem.ToString();
                while(rem < n2)
                {
                    r += "0";
                    total += "0";
                    rem = BigInteger.Parse(r);
                }
                dividents.Push(r);
                BigInteger digit = BigInteger.DivRem(rem, n2, out rem);
                total += digit.ToString();
                rem = BigInteger.Parse(rem.ToString() + "0");
            } while (!rem.Equals(BigInteger.Zero) && (!dividents.Contains(r) || dividents.Count < 2 ));
            return total;
        }

        private static int FindFirstNonZeroDigit(string bigNum)
        {
            for(int i = 0; i < bigNum.Length; i++)
            {
                if (bigNum[i] != '0' && bigNum[i] >= '0' && bigNum[i] <= '9')
                    return i;
            }
            return -1;
        }
    }
}
