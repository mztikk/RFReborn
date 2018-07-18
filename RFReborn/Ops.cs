﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RFReborn
{
    /// <summary>
    /// Provides general operations.
    /// </summary>
    public static class Ops
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
        /// <param name="charset">Characterst to be used when generating strings.</param>
        /// <param name="checkFunc">Function to for the generated string to be checked against.</param>
        /// <param name="startLength">Minimum/Starting length of the generated strings.</param>
        /// <param name="maxLength">Maximum length of the genereated strings.</param>
        /// <returns>The string that matched the <paramref name="checkFunc"/> or <see cref="string.Empty"/> otherwise.</returns>
        public static string BruteForce(IList<char> charset, Func<string, bool> checkFunc, int startLength = 1, int maxLength = 6)
        {
            var found = string.Empty;
            for (var length = startLength; length < maxLength; length++)
            {
                const long startw = 0;
                var endw = (long)Math.Pow(charset.Count, length);
                var d = new long[length + 1];
                var charsetLength = charset.Count;

                for (var i = length; i >= 0; i--)
                {
                    d[i] = (long)Math.Pow(charsetLength, i);
                }

                Parallel.For(startw, endw, (ind, loopState) =>
                {
                    var s = new char[length];
                    var mw = ind;
                    for (var i = length; i >= 0; i--)
                    {
                        var w = (int)(mw / d[i]);

                        if (i == length)
                        {
                            if (w != 0)
                            {
                                s[i] = (charset[w]);
                            }
                        }
                        else
                        {
                            s[i] = (charset[w]);
                        }

                        mw -= (w * d[i]);
                    }

                    var str = new string(s);
                    if (checkFunc(str))
                    {
                        found = str;
                        loopState.Break();
                    }
                });
            }

            return found;
        }
    }
}
