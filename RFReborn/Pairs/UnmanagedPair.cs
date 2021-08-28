using System.Collections.Generic;

namespace RFReborn.Pairs
{
    /// <summary>
    /// Struct that holds two values of unmanaged type as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public readonly struct UnmanagedPair<T> where T : unmanaged
    {
        /// <summary>
        /// Left Value of the StructPair
        /// </summary>
        public readonly T Left;

        /// <summary>
        /// Right Value of the StructPair
        /// </summary>
        public readonly T Right;

        /// <summary>
        /// Creates a new <see cref="UnmanagedPair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public UnmanagedPair(T left, T right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Create a <see cref="UnmanagedPair{T}"/> from a Tuple T, T
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator UnmanagedPair<T>((T left, T right) args) => new(args.left, args.right);

        /// <summary>
        /// Creates a new <see cref="UnmanagedPair{T}"/> from a <see cref="Pair{T}"/>
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator UnmanagedPair<T>(Pair<T> pair) => new(pair.Left, pair.Right);

        /// <summary>
        /// Creates a new <see cref="UnmanagedPair{T}"/> from a <see cref="RefValuePair{T}"/>
        /// </summary>
        /// <param name="refValuePair"></param>
        public static implicit operator UnmanagedPair<T>(RefValuePair<T> refValuePair) => new(refValuePair.Left, refValuePair.Right);
    }

    /// <summary>
    /// Extends <see cref="ICollection{T}"/>.
    /// </summary>
    public static class UnmanagedPairCollection
    {
        /// <summary>
        /// Adds a new <see cref="UnmanagedPair{T}"/> to the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="T">type of <see cref="UnmanagedPair{T}"/></typeparam>
        /// <param name="collection"><see cref="ICollection{T}"/> to add to</param>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public static void Add<T>(this ICollection<UnmanagedPair<T>> collection, T left, T right) where T : unmanaged => collection.Add(new UnmanagedPair<T>(left, right));
    }
}
