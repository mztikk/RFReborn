namespace RFReborn.Pairs
{
    /// <summary>
    /// Ref Struct that holds two values as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public readonly ref struct RefValuePair<T>
    {
        /// <summary>
        /// Left Value of the RefValuePair
        /// </summary>
        public readonly T Left;

        /// <summary>
        /// Right Value of the RefValuePair
        /// </summary>
        public readonly T Right;

        /// <summary>
        /// Creates a new <see cref="RefValuePair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public RefValuePair(T left, T right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Create a <see cref="RefValuePair{T}"/> from a Tuple T, T
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator RefValuePair<T>((T left, T right) args) => new(args.left, args.right);

        /// <summary>
        /// Creates a new <see cref="RefValuePair{T}"/> from a <see cref="Pair{T}"/>
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator RefValuePair<T>(Pair<T> pair) => new(pair.Left, pair.Right);

        /// <summary>
        /// Creates a new <see cref="RefValuePair{T}"/> from a <see cref="ValuePair{T}"/>
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator RefValuePair<T>(ValuePair<T> pair) => new(pair.Left, pair.Right);
    }
}
