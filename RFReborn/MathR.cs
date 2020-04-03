using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RFReborn
{
    /// <summary>
    /// Contains various functions to perform math operations.
    /// </summary>
    public static class MathR
    {
        /// <summary>
        /// Calculates the factorial of n.
        /// </summary>
        /// <param name="n">n.</param>
        /// <returns>n!.</returns>
        public static BigInteger Factorial(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "N can't be smaller than zero.");
            }

            if (n < 2)
            {
                return 1;
            }

            BigInteger sum = n;
            BigInteger result = n;
            for (int i = n - 2; i > 1; i -= 2)
            {
                sum += i;
                result *= sum;
            }

            if (n % 2 != 0)
            {
                result *= (BigInteger)Math.Round((double)n / 2, MidpointRounding.AwayFromZero);
            }

            return result;
        }

        /// <summary>
        /// Calculates the Nth Fibonacci.
        /// </summary>
        /// <param name="n">n.</param>
        /// <returns>fib.</returns>
        public static BigInteger Fibonacci(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "N can't be smaller than zero.");
            }

            BigInteger a = BigInteger.Zero;
            BigInteger b = BigInteger.One;
            for (int i = 31; i >= 0; i--)
            {
                BigInteger d = a * ((b * 2) - a);
                BigInteger e = (a * a) + (b * b);
                a = d;
                b = e;
                if (((n >> i) & 1) != 0)
                {
                    BigInteger c = a + b;
                    a = b;
                    b = c;
                }
            }

            return a;
        }

        /// <summary>
        /// Checks if a number is prime.
        /// </summary>
        /// <param name="n">Number to check.</param>
        /// <returns>TRUE if prime; otherwise FALSE.</returns>
        public static bool IsPrime(long n)
        {
            if (n <= 1)
            {
                return false;
            }

            if (n % 2 == 0)
            {
                return n == 2;
            }

            double boundary = Math.Ceiling(Math.Sqrt(n));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates the Nth prime number.
        /// </summary>
        /// <param name="n">n.</param>
        /// <returns>prime number.</returns>
        public static long NthPrime(int n)
        {
            if (n < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "N can't be smaller than one.");
            }

            if (n == 1)
            {
                return 2;
            }

            long i = 3;
            int primeCount = 1;
            while (primeCount < n)
            {
                if (IsPrime(i))
                {
                    primeCount++;
                }
                i += 2;
            }

            return i - 2;
        }

        /// <summary>
        /// Rotates bits to the left.
        /// </summary>
        /// <param name="x">Value to be rotated.</param>
        /// <param name="r">Number of bits to shift.</param>
        /// <returns>The rotated value. There is no error return.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong x, int r) => (x << r) | (x >> (64 - r));

        /// <summary>
        /// Rotates bits to the left.
        /// </summary>
        /// <param name="x">Value to be rotated.</param>
        /// <param name="r">Number of bits to shift.</param>
        /// <returns>The rotated value. There is no error return.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint x, int r) => (x << r) | (x >> (32 - r));
    }
}
