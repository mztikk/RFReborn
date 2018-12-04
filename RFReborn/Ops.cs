using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RFReborn
{
    /// <summary>
    /// Provides general operations.
    /// </summary>
    public static unsafe class Ops
    {
        /// <summary>
        /// Swaps two variables.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="m1">First value.</param>
        /// <param name="m2">Second value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T m1, ref T m2)
        {
            T tmp = m1;
            m1 = m2;
            m2 = tmp;
        }

        /// <summary>
        /// Swaps <paramref name="len"/> number of bytes at the specified pointers.
        /// </summary>
        /// <param name="m1">First pointer.</param>
        /// <param name="m2">Second pointer.</param>
        /// <param name="len">Number of bytes to swap.</param>
        public static void MemSwap(void* m1, void* m2, int len)
        {
            var pl = (byte*)m1;
            var pr = (byte*)m2;
            var bEnd = pl + len;

            while (pl <= bEnd - 8)
            {
                var temp = *(ulong*)pl;
                *(ulong*)pl = *(ulong*)pr;
                *(ulong*)pr = temp;

                pl += 8;
                pr += 8;
            }

            while (pl <= bEnd - 4)
            {
                var temp = *(uint*)pl;
                *(uint*)pl = *(uint*)pr;
                *(uint*)pr = temp;

                pl += 4;
                pr += 4;
            }

            while (pl < bEnd)
            {
                var temp = *pl;
                *pl = *pr;
                *pr = temp;

                pl++;
                pr++;
            }
        }

        /// <summary>
        /// Merges all given <see cref="IList{T}"/> into one and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="listArray">All <see cref="IList{T}"/> to merge.</param>
        /// <returns>The merged <see cref="IList{T}"/>.</returns>
        public static IList<T> Merge<T>(params IList<T>[] listArray) => Merge(enumList: listArray);

        /// <summary>
        /// Merges all given <see cref="IList{T}"/> into one and returns it.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="enumList"><see cref="IEnumerable{T}"/> of all <see cref="IList{T}"/> to merge.</param>
        /// <returns>The merged <see cref="IList{T}"/>.</returns>
        public static IList<T> Merge<T>(IEnumerable<IList<T>> enumList)
        {
            var newSize = 0;
            foreach (IList<T> list in enumList)
            {
                newSize += list.Count;
            }

            var rtn = new T[newSize];
            var lastIndex = 0;
            foreach (IList<T> list in enumList)
            {
                list.CopyTo(rtn, lastIndex);
                lastIndex += list.Count;
            }

            return rtn;
        }

        /// <summary>
        /// Generates strings based on the specified charset and tests them against the <paramref name="checkFunc"/> until it returns true and returns the matching string.
        /// </summary>
        /// <param name="charset">Characterset to be used when generating strings.</param>
        /// <param name="checkFunc">Function to for the generated string to be checked against.</param>
        /// <param name="startLength">Minimum/Starting length of the generated strings.</param>
        /// <param name="maxLength">Maximum length of the genereated strings.</param>
        /// <returns>The string that matched the <paramref name="checkFunc"/> or <see cref="string.Empty"/> otherwise.</returns>
        public static string BruteForce(string charset, Func<string, bool> checkFunc, int startLength = 1, int maxLength = 6) => BruteForce(charset.ToCharArray(), checkFunc, startLength, maxLength);

        /// <summary>
        /// Generates strings based on the specified charset and tests them against the <paramref name="checkFunc"/> until it returns true and returns the matching string.
        /// </summary>
        /// <param name="charset">Characterset to be used when generating strings.</param>
        /// <param name="checkFunc">Function to for the generated string to be checked against.</param>
        /// <param name="startLength">Minimum/Starting length of the generated strings.</param>
        /// <param name="maxLength">Maximum length of the genereated strings.</param>
        /// <returns>The string that matched the <paramref name="checkFunc"/> or <see cref="string.Empty"/> otherwise.</returns>
        public static string BruteForce(IList<char> charset, Func<string, bool> checkFunc, int startLength = 1, int maxLength = 6)
        {
            var found = string.Empty;
            var charsetLength = charset.Count;
            const long startw = 0;
            var d = new long[maxLength + 1];
            var set = new Dictionary<int, string>(Environment.ProcessorCount);
            for (var length = startLength; length < maxLength; length++)
            {
                var endw = (long)Math.Pow(charset.Count, length);

                for (var i = length; i >= 0; i--)
                {
                    d[i] = (long)Math.Pow(charsetLength, i);
                }

                Parallel.For(startw, endw, (ind, loopState) =>
                {
                    var id = Environment.CurrentManagedThreadId;
                    if (!set.ContainsKey(id) || !set.TryGetValue(id, out var str))
                    {
                        str = new string(' ', length);
                        set.Add(id, str);
                    }

                    var mw = ind;
                    fixed (char* strp = str)
                    {
                        for (var i = length; i >= 0; i--)
                        {
                            var w = (int)(mw / d[i]);

                            if (i == length)
                            {
                                if (w != 0)
                                {
                                    strp[i] = (charset[w]);
                                }
                            }
                            else
                            {
                                strp[i] = (charset[w]);
                            }

                            mw -= (w * d[i]);
                        }

                        if (checkFunc(str))
                        {
                            found = str;
                            loopState.Break();
                        }
                    }
                });

                set.Clear();
            }

            return found;
        }
    }
}
