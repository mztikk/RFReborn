using System;
using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Random"/>.
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
        public static double NextDouble(this Random random, double minValue, double maxValue)
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
        public static double NextDouble(this Random random, double maxValue)
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
        public static T Choice<T>(this Random random, IList<T> list)
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
        public static T Choice<T>(this Random random, IEnumerable<T> enumerable)
        {
            int size = enumerable.Count();
            int i = 0;

            int rng = random.Next(0, size);
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
        /// Chooses a weighted random item inside of the provided <see cref="IList{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="random">Random instance.</param>
        /// <param name="choices">IList to choose random item from.</param>
        /// <param name="weights">Weights to use</param>
        /// <returns>Random item inside of Enumerable, default in case of a failure.>.</returns>
        public static T WeightedChoice<T>(this Random random, IList<T> choices, IList<double> weights)
        {
            double weightSum = 0d;
            for (int i = 0; i < choices.Count; i++)
            {
                weightSum += weights[i];
            }

            double rnd = random.NextDouble(weightSum);
            for (int i = 0; i < choices.Count; i++)
            {
                if (rnd < weights[i])
                {
                    return choices[i];
                }

                rnd -= weights[i];
            }

            throw new Exception();
        }

        /// <summary>
        /// Fills the elements of a specified array of int with random numbers.
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="buffer">An array of int to contain random numbers.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="buffer"/> is null.
        /// </exception>
        public static void NextInts(this Random random, int[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            byte[] randomBytes = new byte[buffer.Length * 4];
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
        public static void NextT<T>(this Random random, T[] buffer) where T : unmanaged
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            byte[] randomBytes = new byte[buffer.Length * sizeof(T)];
            random.NextBytes(randomBytes);
            //Buffer.BlockCopy(randomBytes, 0, buffer, 0, randomBytes.Length);
            fixed (void* rp = randomBytes, bp = buffer)
            {
                Buffer.MemoryCopy(rp, bp, randomBytes.Length, randomBytes.Length);
            }
        }

        /// <summary>
        /// Returns a random value of type T
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="random">Random provider</param>
        public static T Next<T>(this Random random) where T : unmanaged
        {
            byte[] buffer = new byte[sizeof(T)];
            fixed (void* ptr = buffer)
            {
                random.NextBytes(buffer);
                return *(T*)ptr;
            }
        }

        /// <summary>
        /// Returns a random string constructed out of a <paramref name="charset"/> with a length of <paramref name="len"/>
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="charset">Chars to use</param>
        /// <param name="len">Length of constructed string</param>
        public static string NextString(this Random random, ReadOnlySpan<char> charset, int len)
        {
            char* rtn = stackalloc char[len];
            for (int i = 0; i < len; i++)
            {
                int rnd = random.Next(0, charset.Length);
                rtn[i] = charset[rnd];
            }

            return new string(rtn);
        }

        /// <summary>
        /// Returns a random string constructed out of all chars except for whitespace and a length between 1 and 31
        /// </summary>
        /// <param name="random">Random provider</param>
        public static string NextString(this Random random) => NextString(random, StringR.Chars, random.Next(1, 32));

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A 64-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        public static long Next(this Random random, long minValue, long maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            ulong uRange = (ulong)(maxValue - minValue);

            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - (((ulong.MaxValue % uRange) + 1) % uRange));

            return (long)(ulongRand % uRange) + minValue;
        }

        /// <summary>
        /// Randomizes the chars in a string based on the char, alpha chars will only be randomized to other alpha chars, numbers only to numbers etc.
        /// </summary>
        /// <param name="random">Random provider</param>
        /// <param name="str">String to randomize</param>
        /// <param name="alpha">Randomize alpha chars, default true</param>
        /// <param name="numeric">Randomize numeric chars, default true</param>
        /// <param name="special">Randomize special characters, default false</param>
        public static void Randomize(this Random random, string str, bool alpha = true, bool numeric = true, bool special = false)
        {
            fixed (void* voidptr = str)
            {
                char* charptr = (char*)voidptr;
                char* end = charptr + str.Length;
                while (charptr < end)
                {
                    if (char.IsLetter(*charptr) && alpha)
                    {
                        if (char.IsUpper(*charptr))
                        {
                            *charptr = random.Choice(StringR.AlphabetUpper);
                        }
                        else if (char.IsLower(*charptr))
                        {
                            *charptr = random.Choice(StringR.AlphabetLower);
                        }
                    }
                    else if (char.IsDigit(*charptr) && numeric)
                    {
                        *charptr = random.Choice(StringR.Numbers);
                    }
                    else if ((char.IsSymbol(*charptr) || char.IsPunctuation(*charptr) || char.IsSeparator(*charptr)) && special)
                    {
                        *charptr = random.Choice(StringR.Special);
                    }

                    charptr++;
                }
            }
        }
    }
}
