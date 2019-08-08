namespace RFReborn
{
    /// <summary>
    /// Struct that holds two values as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public readonly struct StructPair<T>
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
        /// Creates a new <see cref="StructPair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public StructPair(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }
}
