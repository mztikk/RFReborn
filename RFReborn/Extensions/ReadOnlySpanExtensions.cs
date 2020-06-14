using System;
using System.Runtime.CompilerServices;
using RFReborn.Comparison;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    public static class ReadOnlySpanExtensions
    {
        /// <summary>
        /// Reverses the elements in a span of unmanaged type.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="span">Span to reverse</param>
        public static unsafe void FastReverse<T>(this ReadOnlySpan<T> span) where T : unmanaged
        {
            fixed (T* ptr = span)
            {
                PointerOperations.Reverse(ptr, span.Length);
            }
        }

        /// <summary>
        /// Checks if the specified <see cref="ReadOnlySpan{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FastEquals<T>(this ReadOnlySpan<T> left, ReadOnlySpan<T> right) where T : unmanaged => FastCompare.Equals(left, right);
    }
}
