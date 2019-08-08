namespace RFReborn
{
    /// <summary>
    /// Class that holds two values as a pair.
    /// </summary>
    /// <typeparam name="T">type of value</typeparam>
    public class ValuePair<T>
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
        /// Creates a new <see cref="ValuePair{T}"/> instance
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public ValuePair(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }
}
