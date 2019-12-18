using System;
using System.Runtime.CompilerServices;
using RFReborn.Comparison;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Span{T}"/>.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Reverses the elements in a span of unmanaged type. 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="span">Span to reverse</param>
        public static unsafe void FastReverse<T>(this Span<T> span) where T : unmanaged
        {
            fixed (T* ptr = span)
            {
                PointerOperations.Reverse(ptr, span.Length);
            }
        }

        /// <summary>
        /// Checks if the specified <see cref="Span{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="Span{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="Span{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FastEquals<T>(this Span<T> left, Span<T> right) where T : unmanaged => FastCompare.Equals(left, right);

        /// <summary>
        /// Checks if the specified <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="Span{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FastEquals<T>(this Span<T> left, ReadOnlySpan<T> right) where T : unmanaged => FastCompare.Equals(left, right);
    }
}
