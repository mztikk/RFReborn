using RFReborn.Comparison;
using System;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Searches for the specified object in a range of elements of a <see cref="Span{T}"/>, and returns the index of its first occurrence.
        /// The range extends from a specified index to the end of the <see cref="Span{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the <see cref="Span{T}"/>.</typeparam>
        /// <param name="span">The <see cref="Span{T}"/> to search.</param>
        /// <param name="value">The object/value to locate</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value"/> within the range of elements in <paramref name="span"/> that extends from <paramref name="startIndex"/> to the last element, if found; otherwise, -1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this Span<T> span, T value, int startIndex) where T : IEquatable<T> => span.Slice(startIndex).IndexOf(value);
    }
}
