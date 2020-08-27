using System.Collections.Generic;

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
        public static double Mean(this IEnumerable<sbyte> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (sbyte item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<byte> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (byte item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<short> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (short item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<ushort> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (ushort item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<int> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (int item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<uint> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (uint item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<long> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (long item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<ulong> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (ulong item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<float> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (float item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }

        /// <summary>
        /// Calculates the arithmetic mean of <paramref name="ienum"/>
        /// </summary>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to calculate the mean from</param>
        /// <returns>Returns the arithmetic mean of <paramref name="ienum"/></returns>
        public static double Mean(this IEnumerable<double> ienum)
        {
            double sum = 0.0;
            ulong count = 0;

            foreach (double item in ienum)
            {
                count++;
                sum += item;
            }

            return (sum / count);
        }
    }
}
