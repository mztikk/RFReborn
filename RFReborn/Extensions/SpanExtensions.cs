﻿using System;

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
    }
}
