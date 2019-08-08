using System;

namespace RFReborn
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
        public static implicit operator Pair<T>((T left, T right)args)
        {
            return new Pair<T>(args.left, args.right);
        }
    }
}
