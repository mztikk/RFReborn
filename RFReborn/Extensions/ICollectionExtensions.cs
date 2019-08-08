using System.Collections.Generic;
using RFReborn.Pairs;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="ICollection{T}"/>.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds a new <see cref="ValuePair{T}"/> to the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="T">type of <see cref="ValuePair{T}"/></typeparam>
        /// <param name="collection"><see cref="ICollection{T}"/> to add to</param>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        public static void Add<T>(this ICollection<ValuePair<T>> collection, T left, T right)
        {
            collection.Add(new ValuePair<T>(left, right));
        }

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
