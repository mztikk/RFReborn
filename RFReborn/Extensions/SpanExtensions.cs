using System;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="string"/>.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Reverses the elements in an array of unmanaged type. 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="array">Array to reverse</param>
        public static unsafe void FastReverse<T>(this Span<T> array) where T : unmanaged
        {
            fixed (T* ptr = array)
            {
                PointerOperations.Reverse(ptr, array.Length);
            }
        }
    }
}
