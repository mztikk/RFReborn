﻿using System;
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
            if (ienum is ICollection<T> icoll)
            {
                return icoll.Count > 0;
            }

            foreach (T _ in ienum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if <paramref name="ienum"/> contains any items matching a specified <see cref="Func{T, TResult}"/> <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to check</param>
        /// <param name="selector">Selector to match items</param>
        /// <returns>Returns TRUE if it contains any item; FALSE otherwise.</returns>
        public static bool Any<T>(this IEnumerable<T> ienum, Func<T, bool> selector)
        {
            foreach (T item in ienum)
            {
                if (selector(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the number of elements in <paramref name="ienum"/>.
        /// Tries to cast to <see cref="ICollection{T}"/> to get access to the Count property which should be its size,
        /// otherwise it loops through the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to get Size of</param>
        /// <returns>Returns the number of elements in <paramref name="ienum"/>.</returns>
        public static int Count<T>(this IEnumerable<T> ienum)
        {
            if (ienum is ICollection<T> icoll)
            {
                return icoll.Count;
            }

            int count = 0;
            foreach (T item in ienum)
            {
                count++;
            }

            return count;
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

            Dictionary<T, ulong> count = new Dictionary<T, ulong>();
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

        /// <summary>
        /// Creates a Lookup <see cref="Dictionary{TKey, TValue}"/> based on the <see cref="IEnumerable{T}"/> <paramref name="ienum"/>, items have to be unique.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to convert</param>
        public static IDictionary<T, bool> ToLookup<T>(this IEnumerable<T> ienum)
        {
            Dictionary<T, bool> rtn = new Dictionary<T, bool>();
            foreach (T item in ienum)
            {
                rtn.Add(item, true);
            }

            return rtn;
        }

        /// <summary>
        /// Calls a <see cref="Func{T, TResult}"/> on every item in a <see cref="IEnumerable{T}"/> and yields the result of the call instead of the original value
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="TResult">Type of <see cref="Func{T, TResult}"/> Result</typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to iteratre</param>
        /// <param name="func"><see cref="Func{T, TResult}"/> to call</param>
        public static IEnumerable<TResult> Call<T, TResult>(this IEnumerable<T> ienum, Func<T, TResult> func)
        {
            foreach (T item in ienum)
            {
                yield return func(item);
            }
        }

        /// <summary>
        /// Calls a <see cref="Action{T}"/> on every item in a <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="ienum"><see cref="IEnumerable{T}"/> to iteratre</param>
        /// <param name="action"><see cref="Action{T}"/> to call</param>
        public static void Call<T>(this IEnumerable<T> ienum, Action<T> action)
        {
            foreach (T item in ienum)
            {
                action(item);
            }
        }
    }
}
