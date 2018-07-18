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
    }
}
