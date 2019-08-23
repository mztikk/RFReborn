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
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static int Mean(this IEnumerable<int> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (int)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static uint Mean(this IEnumerable<uint> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (uint)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static long Mean(this IEnumerable<long> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (long)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static ulong Mean(this IEnumerable<ulong> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (ulong)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static byte Mean(this IEnumerable<byte> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (byte)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static short Mean(this IEnumerable<short> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (short)(sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static ushort Mean(this IEnumerable<ushort> ienum)
        {
            var sum = new BigInteger(0);
            ulong count = 0;

            foreach (var item in ienum)
            {
                count++;
                sum += item;
            }

            return (ushort)(sum / count);
        }
    }
}
