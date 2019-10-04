using System;
using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
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
        /// Reverses the elements in a <see cref="string"/>.
        /// </summary>
        /// <param name="str"><see cref="string"/> to reverse</param>
        public static unsafe void FastReverse(this string str)
        {
            fixed (char* ptr = str)
            {
                PointerOperations.Reverse(ptr, str.Length);
            }
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// It will check for exceeding the actual length of the string or whats left after the start index
        /// </summary>
        /// <param name="input">String to get substring from</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="len">The number of characters in the substring.</param>
        public static string SafeSubstring(this string input, int startIndex, int len)
        {
            int maxLen = input.Length - startIndex;
            int realLen = Math.Min(maxLen, len);
            return input.Substring(startIndex, realLen);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and continues to the end of the string.
        /// It will check for exceeding the actual length of the string or whats left after the start index
        /// </summary>
        /// <param name="input">String to get substring from</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        public static string SafeSubstring(this string input, int startIndex) => input.SafeSubstring(startIndex, input.Length);
    }
}
