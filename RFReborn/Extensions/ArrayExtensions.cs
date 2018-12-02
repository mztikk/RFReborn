using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="System.Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Checks if the specified arrays are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First array to compare.</param>
        /// <param name="right">Second array to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FastEquals<T>(this T[] left, T[] right) where T : unmanaged
        {
            return FastCompare.Equals(left, right);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of its first occurrence in an array of unmanaged type.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="haystack">Array to search in.</param>
        /// <param name="needle">Item in array to search for.</param>
        /// <param name="startIndex">Index in array at which to start searching, default is 0.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, –1.</returns>
        public static unsafe int FastIndexOf<T>(this T[] haystack, T needle, int startIndex = 0) where T : unmanaged
        {
            var found = -1;
            var size = sizeof(T);
            fixed (void* vhp = haystack)
            {
                T* np = &needle;
                var hp = (byte*)vhp;
                var traverse = hp + (startIndex * size);
                var end = hp + ((haystack.Length - 1) * size);
                while (traverse <= end)
                {
                    if (FastCompare.Equals(traverse, np, size))
                    {
                        found = (int)((traverse - hp) / size);
                        break;
                    }

                    traverse += size;
                }
            }

            return found;
        }

        /// <summary>
        /// Searches for the specified object and returns the index of its last occurrence in an array of unmanaged type.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="haystack">Array to search in.</param>
        /// <param name="needle">The object to locate in the array/>.</param>
        /// <returns>The zero-based index of the last occurrence of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, –1.</returns>
        public static unsafe int FastLastIndexOf<T>(this T[] haystack, T needle) where T : unmanaged
        {
            var found = -1;
            var size = sizeof(T);
            fixed (void* vhp = haystack)
            {
                T* np = &needle;
                var hp = (byte*)vhp;
                var end = hp + (haystack.Length * size);
                end -= size;
                while (end >= hp)
                {
                    if (FastCompare.Equals(end, np, size))
                    {
                        found = (int)((end - hp) / size);
                        break;
                    }

                    end -= size;
                }
            }

            return found;
        }

        /// <summary>
        /// Searches for the specified object and returns the indices of all its occurrences in an array of unmanaged type.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="haystack">Array to search in.</param>
        /// <param name="needle">The object to locate in the array/>.</param>
        /// <returns>The zero-based indices of the occurrences of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, an empty list.</returns>
        public static IEnumerable<int> IndicesOf<T>(this T[] haystack, T needle) where T : unmanaged
        {
            var indices = new List<int>();

            var i = 0;
            var end = haystack.Length;
            while (i <= end)
            {
                var index = haystack.FastIndexOf(needle, i);
                if (index == -1)
                {
                    break;
                }

                i = index + 1;
                indices.Add(index);
            }

            return indices;
        }
    }
}
