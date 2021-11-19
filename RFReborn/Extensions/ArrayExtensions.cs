using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RFReborn.Comparison;

namespace RFReborn.Extensions;

/// <summary>
/// Extends <see cref="Array"/>.
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
    public static bool FastEquals<T>(this T[] left, T[] right) where T : unmanaged => FastCompare.Equals(left, right);

    /// <summary>
    /// Searches for the specified object and returns the indices of all its occurrences in an array of unmanaged type.
    /// </summary>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <param name="haystack">Array to search in.</param>
    /// <param name="needle">The object to locate in the array/>.</param>
    /// <returns>The zero-based indices of the occurrences of <paramref name="needle"/> in the entire <paramref name="haystack"/>, if found; otherwise, an empty list.</returns>
    public static IEnumerable<int> IndicesOf<T>(this T[] haystack, T needle) where T : unmanaged
    {
        int i = 0;
        while (i < haystack.Length)
        {
            int index = Array.IndexOf(haystack, needle, i);
            if (index == -1)
            {
                break;
            }

            yield return index;
            i = index + 1;
        }
    }

    /// <summary>
    /// Reverses the elements in an array of unmanaged type.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="array">Array to reverse</param>
    public static unsafe void FastReverse<T>(this T[] array) where T : unmanaged
    {
        fixed (T* ptr = array)
        {
            PointerOperations.Reverse(ptr, array.Length);
        }
    }
}
