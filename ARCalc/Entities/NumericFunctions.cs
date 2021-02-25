using ARCalc.Exceptions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ARCalc.Entities
{
    public class NumericFunctions
    {

        /// <summary>
        /// Compute the factorial of natural number num
        /// </summary>
        /// <param name="num">Natural number represented as string (bigNum)</param>
        /// <returns></returns>
        /// <exception cref="IllegalNumberException"></exception>
        public static BigInteger Factofial(string num)
        {
            num = StringNumber.RemoveSpaces(num);
            num = StringNumber.RemoveUnnecessaryZeros(num);
            if (num.Equals("0") || num.Equals("1"))
                return BigInteger.One;
            if (num.Contains('.'))
            {
                throw new IllegalNumberException($"Illegal number. Expected Natural number, but found Rational: {num}");
            }
            else if (num.Contains('-'))
            {
                throw new IllegalNumberException($"Illegal number. Expected Natural number, but found negaive Integer: {num}");
            }
            BigInteger n = BigInteger.Parse(num);
            BigInteger res = BigInteger.Parse(num);
            while(n > BigInteger.One)
            {
                n = BigInteger.Subtract(n, BigInteger.One);
                res = BigInteger.Multiply(res, n);
            }
            return res;
        }
        /// <summary>
        /// Computes ith Fibonachi number.
        /// </summary>
        /// <param name="i">The number of position at Fibonachi numbers series (i-th)</param>
        /// <returns></returns>
        /// <exception cref="IllegalNumberException"></exception>
        public static BigInteger GetFibboNumber(string i)
        {
            i = StringNumber.RemoveSpaces(i);
            i = StringNumber.RemoveUnnecessaryZeros(i);
            if (i.Equals("0"))
                return BigInteger.Zero;
            else if (i.Equals("1"))
                return BigInteger.One;
            if (i.Contains('.'))
            {
                throw new IllegalNumberException($"Illegal number. Expected Natural number, but found Rational: {i}");
            }
            else if (i.Contains('-'))
            {
                throw new IllegalNumberException($"Illegal number. Expected Natural number, but found negaive Integer: {i}");
            }

            /* Compute ith Fibonachi number */
            BigInteger f1 = BigInteger.Zero;
            BigInteger f2 = BigInteger.One;
            BigInteger res = BigInteger.Zero;
            BigInteger n = BigInteger.Parse(i);
            BigInteger n_i = BigInteger.One;
            while(n_i < n)
            {
                res = BigInteger.Add(f1, f2);
                f1 = f2;
                f2 = res;
                n_i = BigInteger.Add(n_i, BigInteger.One);
            }
            return res;
        }
    }
}
