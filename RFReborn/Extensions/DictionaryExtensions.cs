using System;
using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Dictionary{TKey, TValue}"/>
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Determines whether the Dictionary contains the specified key using the specified <see cref="StringComparison"/> to compare keys.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="dict">Dictionary to check</param>
        /// <param name="key">The key to locate in the Dictionary</param>
        /// <param name="stringComparison"><see cref="StringComparison"/> to use</param>
        /// <param name="actualKey">Will have the value of the actual key if it got matched with <see cref="StringComparison.OrdinalIgnoreCase"/> for example</param>
        /// <returns>TRUE if the Dictionary contains an element with the specified key; otherwise, FALSE.</returns>
        public static bool ContainsKey<T>(this Dictionary<string, T> dict, string key, StringComparison stringComparison, out string actualKey)
        {
            foreach (var k in dict.Keys)
            {
                if (string.Equals(k, key, stringComparison))
                {
                    actualKey = k;
                    return true;
                }
            }

            actualKey = null;
            return false;
        }
    }
}
