using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="System.Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Checks if the specified arrays are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First array to compare.</param>
        /// <param name="right">Second array to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FastEquals<T>(this T[] left, T[] right) where T : unmanaged => FastCompare.Equals(left, right);

        /// <summary>
        /// Searches for the specified object and returns the indices of all its occurrences in an array of unmanaged type.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="haystack">Array to search in.</param>
        /// <param name="needle">The object to locate in the array/>.</param>
        /// <returns>The zero-based indices of the occurrences of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, an empty list.</returns>
        public static IEnumerable<int> IndicesOf<T>(this T[] haystack, T needle) where T : unmanaged
        {
            List<int> indices = new List<int>();

            int i = 0;
            int end = haystack.Length;
            while (i < end)
            {
                //var index = haystack.FastIndexOf(needle, i);
                int index = Array.IndexOf(haystack, needle, i);
                if (index == -1)
                {
                    break;
                }

                i = index + 1;
                indices.Add(index);
            }

            return indices;
        }

        /// <summary>
        /// Searches for the specified object and returns the indices of all its occurrences in an array of unmanaged type.
        /// </summary>
        /// <param name="haystack">Array to search in.</param>
        /// <param name="needle">The object to locate in the array/>.</param>
        /// <returns>The zero-based indices of the occurrences of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, an empty list.</returns>
        public static IEnumerable<int> IndicesOf(this string haystack, char needle)
        {
            List<int> indices = new List<int>();

            int i = 0;
            int end = haystack.Length;
            while (i < end)
            {
                int index = haystack.IndexOf(needle, i);
                if (index == -1)
                {
                    break;
                }

                i = index + 1;
                indices.Add(index);
            }

            return indices;
        }

        /// <summary>
        /// Reverses the elements in an array of unmanaged type. 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="array">Array to reverse</param>
        public static unsafe void FastReverse<T>(this T[] array) where T : unmanaged
        {
            fixed (T* ptr = array)
            {
                Reverse(ptr, array.Length);
            }
        }

        internal static unsafe void Reverse<T>(T* ptr, int len) where T : unmanaged
        {
            T* start = ptr;
            T* end = start + len - 1;
            while (start < end)
            {
                T tmp = *end;
                *end = *start;
                *start = tmp;
                start++;
                end--;
            }
        }
    }
}
