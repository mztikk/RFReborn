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
    }
}
