using System;
using System.Collections.Generic;
using System.Linq;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="System.Random"/>.
    /// </summary>
    public static unsafe class RandomExtensions
    {
        /// <summary>
        /// Chooses a random item inside of the provided <see cref="IList{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="random">Random instance.</param>
        /// <param name="list">List to choose random item from.</param>
        /// <returns>Random item inside of list, default in case of a failure.</returns>
        public static T Choice<T>(this System.Random random, IList<T> list)
        {
            if (list.Count <= 0)
            {
                return default;
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
            // Linq Count() tries to cast to ICollection so I merged the ICollection overload with this one
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

            return default;
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
