using System;

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
    }
}
