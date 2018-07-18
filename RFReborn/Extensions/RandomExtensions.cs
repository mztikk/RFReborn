using System.Collections.Generic;
using System.Linq;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="System.Random"/>
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Chooses a random item inside of the provided <see cref="IList{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="random">Random instance</param>
        /// <param name="list">List to choose random item from</param>
        /// <returns>Random item inside of list, default in case of a failure.</returns>
        public static T Choice<T>(this System.Random random, IList<T> list)
        {
            if (list.Count <= 0)
            {
                return default;
            }

            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Chooses a random item inside of the provided <see cref="IEnumerable{T}"/> and returns it.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="random">Random instance</param>
        /// <param name="enumerable">Enumerable to choose random item from</param>
        /// <returns>Random item inside of Enumerable, default in case of a failure.></returns>
        public static T Choice<T>(this System.Random random, IEnumerable<T> enumerable)
        {
            // Linq Count() tries to cast to ICollection so I merged the ICollection overload with this one
            var size = enumerable.Count();
            var i = 0;

            var rng = random.Next(0, size);
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (i == rng)
                    {
                        return enumerator.Current;
                    }

                    i++;
                }
            }

            return default;
        }
    }
}
