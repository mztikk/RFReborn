using System.Collections.Generic;

namespace RFReborn.Pairs
{
    /// <summary>
    /// Struct that holds two values as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public readonly struct ValuePair<T>
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
        /// Creates a new <see cref="ValuePair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public ValuePair(T left, T right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Create a <see cref="ValuePair{T}"/> from a Tuple T, T
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator ValuePair<T>((T left, T right) args) => new ValuePair<T>(args.left, args.right);

        /// <summary>
        /// Creates a new <see cref="ValuePair{T}"/> from a <see cref="Pair{T}"/>
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator ValuePair<T>(Pair<T> pair) => new ValuePair<T>(pair.Left, pair.Right);

        /// <summary>
        /// Creates a new <see cref="ValuePair{T}"/> from a <see cref="RefValuePair{T}"/>
        /// </summary>
        /// <param name="refValuePair"></param>
        public static implicit operator ValuePair<T>(RefValuePair<T> refValuePair) => new ValuePair<T>(refValuePair.Left, refValuePair.Right);
    }

    /// <summary>
    /// Extends <see cref="ICollection{T}"/>.
    /// </summary>
    public static class ValuePairCollection
    {
        /// <summary>
        /// Adds a new <see cref="ValuePair{T}"/> to the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="T">type of <see cref="ValuePair{T}"/></typeparam>
        /// <param name="collection"><see cref="ICollection{T}"/> to add to</param>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public static void Add<T>(this ICollection<ValuePair<T>> collection, T left, T right) => collection.Add(new ValuePair<T>(left, right));
    }
}
