using System.Collections.Generic;
using System.Numerics;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="IEnumerable{T}"/> with Arithmetic Mean calculation methods
    /// </summary>
    public static class ArithmeticMean
    {
        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static sbyte Mean(this IEnumerable<sbyte> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (sbyte item in ienum)
            {
                count++;
                sum += item;
            }

            return (sbyte)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static byte Mean(this IEnumerable<byte> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (byte item in ienum)
            {
                count++;
                sum += item;
            }

            return (byte)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static short Mean(this IEnumerable<short> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (short item in ienum)
            {
                count++;
                sum += item;
            }

            return (short)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static ushort Mean(this IEnumerable<ushort> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (ushort item in ienum)
            {
                count++;
                sum += item;
            }

            return (ushort)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static int Mean(this IEnumerable<int> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (int item in ienum)
            {
                count++;
                sum += item;
            }

            return (int)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static uint Mean(this IEnumerable<uint> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (uint item in ienum)
            {
                count++;
                sum += item;
            }

            return (uint)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static long Mean(this IEnumerable<long> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (long item in ienum)
            {
                count++;
                sum += item;
            }

            return (long)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static ulong Mean(this IEnumerable<ulong> ienum)
        {
            BigInteger sum = new BigInteger(0);
            ulong count = 0;

            foreach (ulong item in ienum)
            {
                count++;
                sum += item;
            }

            return (ulong)(sum / count);
        }
    }
}
