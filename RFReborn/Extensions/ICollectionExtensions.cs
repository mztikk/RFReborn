using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="ICollection{T}"/>.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds a new <see cref="KeyValuePair{TKey, TValue}"/> to the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="TKey">type of key from <see cref="KeyValuePair{TKey, TValue}"/></typeparam>
        /// <typeparam name="TValue">type of value from <see cref="KeyValuePair{TKey, TValue}"/></typeparam>
        /// <param name="collection"><see cref="ICollection{T}"/> to add to</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void Add<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> collection, TKey key, TValue value) => collection.Add(new KeyValuePair<TKey, TValue>(key, value));
    }
}
