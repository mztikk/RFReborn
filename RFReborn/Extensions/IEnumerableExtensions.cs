using System;
using System.Collections.Generic;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        ///     Returns a readable string of the objects inside an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="iEnumerable">The IEnumerable.</param>
        /// <typeparam name="T">The type of the IEnumerable.</typeparam>
        /// <returns></returns>
        public static string ToObjectsString<T>(this IEnumerable<T> iEnumerable) => "{ " + string.Join(", ", iEnumerable) + " }";

        /// <summary>
        /// Checks if <paramref name="ienum"/> contains any items.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to check</param>
        /// <returns>Returns TRUE if it contains any item; FALSE otherwise.</returns>
        public static bool Any<T>(this IEnumerable<T> ienum)
        {
            foreach (T item in ienum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the mode of <paramref name="ienum"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ienum"></param>
        /// <returns></returns>
        public static T Mode<T>(this IEnumerable<T> ienum)
        {
            if (!ienum.Any())
            {
                throw new ArgumentException("IEnumerable can not be empty.");
            }

            var count = new Dictionary<T, ulong>();
            foreach (T item in ienum)
            {
                count.AddOrIncrement(item);
            }

            KeyValuePair<T, ulong> highest = new KeyValuePair<T, ulong>(default, 0);
            foreach (KeyValuePair<T, ulong> item in count)
            {
                if (item.Value > highest.Value)
                {
                    highest = item;
                }
            }

            return highest.Key;
        }
    }
}
