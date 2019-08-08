namespace RFReborn
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
        public static implicit operator ValuePair<T>((T left, T right) args)
        {
            return new ValuePair<T>(args.left, args.right);
        }

        /// <summary>
        /// Creates a new <see cref="ValuePair{T}"/> from a <see cref="Pair{T}"/>
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator ValuePair<T>(Pair<T> pair)
        {
            return new ValuePair<T>(pair.Left, pair.Right);
        }
    }
}
