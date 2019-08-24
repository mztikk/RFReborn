﻿using System;
using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="System.Random"/>.
    /// </summary>
    public static unsafe class RandomExtensions
    {
        /// <summary>
        /// Returns a random <see cref="double"/> that is within a specified range.
        /// </summary>
        /// <param name="random">Random generator to be used.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A <see cref="double"/> greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
        /// <exception cref="ArgumentException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        public static double NextDouble(this System.Random random, double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException($"{nameof(minValue)} is greater than {nameof(maxValue)}.");
            }
            if (minValue == maxValue)
            {
                return minValue;
            }

            return (random.NextDouble() * (maxValue - minValue)) + minValue;
        }

        /// <summary>
        /// Returns a non-negative random <see cref="double"/> that is less than the specified maximum.
        /// </summary>
        /// <param name="random">Random generator to be used.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        /// <returns>A <see cref="double"/> that is greater than or equal to 0, and less than maxValue; that is, the range of return values ordinarily includes 0 but not maxValue. However, if maxValue equals 0, maxValue is returned.</returns>
        /// <exception cref="ArgumentException"><paramref name="maxValue"/> is less than 0.</exception>
        public static double NextDouble(this System.Random random, double maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentException($"{nameof(maxValue)} is less than 0.");
            }

            return NextDouble(random, 0, maxValue);
        }

        /// <summary>
        /// Chooses a random item inside of the provided <see cref="IList{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="random">Random instance.</param>
        /// <param name="list">List to choose random item from.</param>
        /// <returns>Random item inside of list, default in case of a failure.</returns>
        public static T Choice<T>(this System.Random random, IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Number of choices can not be lesser than or equal to 0.");
            }

            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Chooses a random item inside of the provided <see cref="IEnumerable{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="random">Random instance.</param>
        /// <param name="enumerable">Enumerable to choose random item from.</param>
        /// <returns>Random item inside of Enumerable, default in case of a failure.>.</returns>
        public static T Choice<T>(this System.Random random, IEnumerable<T> enumerable)
        {
            var size = enumerable.Count();
            var i = 0;

            var rng = random.Next(0, size);
            foreach (T item in enumerable)
            {
                if (i == rng)
                {
                    return item;
                }

                i++;
            }

            // should never get here
            // only possible if random.Next returns wrong values or the size of the enumerable wasn't generated correctly
            throw new Exception($"Failed to get random item in range of 0 to {size} of {enumerable}");
        }

        /// <summary>
        /// Fills the elements of a specified array of int with random numbers.
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="buffer">An array of int to contain random numbers.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="buffer"/> is null.
        /// </exception>
        public static void NextInts(this System.Random random, int[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var randomBytes = new byte[buffer.Length * 4];
            random.NextBytes(randomBytes);
            //Buffer.BlockCopy(randomBytes, 0, buffer, 0, randomBytes.Length);
            fixed (void* rp = randomBytes, bp = buffer)
            {
                Buffer.MemoryCopy(rp, bp, randomBytes.Length, randomBytes.Length);
            }
        }

        /// <summary>
        /// Fills the elements of a specified array of unmanaged type with random numbers.
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="buffer">An array of unmanaged type to contain random bytes converted to T.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="buffer"/> is null.
        /// </exception>
        public static void NextT<T>(this System.Random random, T[] buffer) where T : unmanaged
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var randomBytes = new byte[buffer.Length * sizeof(T)];
            random.NextBytes(randomBytes);
            //Buffer.BlockCopy(randomBytes, 0, buffer, 0, randomBytes.Length);
            fixed (void* rp = randomBytes, bp = buffer)
            {
                Buffer.MemoryCopy(rp, bp, randomBytes.Length, randomBytes.Length);
            }
        }
    }
}
