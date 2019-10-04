using System;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Reverses the elements in a <see cref="string"/>.
        /// </summary>
        /// <param name="str"><see cref="string"/> to reverse</param>
        public static unsafe void FastReverse(this string str)
        {
            fixed (char* ptr = str)
            {
                ArrayExtensions.Reverse(ptr, str.Length);
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
