using System.Collections.Generic;

namespace RFReborn.Pairs
{
    /// <summary>
    /// Class that holds two values as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public class Pair<T>
    {
        /// <summary>
        /// Left Value of the ValuePair
        /// </summary>
        public T Left;

        /// <summary>
        /// Right Value of the ValuePair
        /// </summary>
        public T Right;

        /// <summary>
        /// Creates a new <see cref="Pair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public Pair(T left, T right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Create a <see cref="Pair{T}"/> from a Tuple T, T
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator Pair<T>((T left, T right) args)
        {
            return new Pair<T>(args.left, args.right);
        }

        /// <summary>
        /// Creates a new <see cref="Pair{T}"/> from a <see cref="ValuePair{T}"/>
        /// </summary>
        /// <param name="valuePair"></param>
        public static implicit operator Pair<T>(ValuePair<T> valuePair)
        {
            return new Pair<T>(valuePair.Left, valuePair.Right);
        }

        /// <summary>
        /// Creates a new <see cref="Pair{T}"/> from a <see cref="RefValuePair{T}"/>
        /// </summary>
        /// <param name="refValuePair"></param>
        public static implicit operator Pair<T>(RefValuePair<T> refValuePair)
        {
            return new Pair<T>(refValuePair.Left, refValuePair.Right);
        }
    }

    /// <summary>
    /// Extends <see cref="ICollection{T}"/>.
    /// </summary>
    public static class PairCollection
    {
        /// <summary>
        /// Adds a new <see cref="Pair{T}"/> to the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="T">type of <see cref="Pair{T}"/></typeparam>
        /// <param name="collection"><see cref="ICollection{T}"/> to add to</param>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public static void Add<T>(this ICollection<Pair<T>> collection, T left, T right)
        {
            collection.Add(new Pair<T>(left, right));
        }
    }
}
