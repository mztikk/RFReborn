﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RFReborn.Extensions;

/// <summary>
/// Extends <see cref="IList{T}"/>.
/// </summary>
public static class IListExtensions
{
    /// <summary>
    ///     Performs a Fisher-Yates / Knuth - Shuffle on the <see cref="IList{T}"/>.
    /// </summary>
    /// <param name="list"><see cref="IList{T}"/> to be shuffled.</param>
    /// <typeparam name="T">Type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FisherYatesShuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T tmp = list[i];
            int r = InternalUtils.s_rng.Next(i, list.Count);
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    /// <summary>
    /// Swaps two members of an <see cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="list"><see cref="IList{T}"/> to perform operation on.</param>
    /// <param name="index1">Index of first member to swap.</param>
    /// <param name="index2">Index of second member to swap.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<T>(this IList<T> list, int index1, int index2)
    {
        if (index1 == index2)
        {
            return;
        }

        (list[index2], list[index1]) = (list[index1], list[index2]);
    }

    /// <summary>
    ///     Generates all permutations for a set of items, including duplicates.
    /// </summary>
    /// <param name="list">
    ///     Items to get permutations from.
    /// </param>
    /// <typeparam name="T">Type.</typeparam>
    /// <returns><see cref="IEnumerable{T}"/> of all permutations.</returns>
    public static IEnumerable<T[]> Permutations<T>(this IList<T> list)
    {
        T[] a = new T[list.Count];
        list.CopyTo(a, 0);

        yield return a;

        int n = a.Length;
        int[] p = new int[n];

        int i = 1;

        while (i < n)
        {
            if (p[i] < i)
            {
                int j = i % 2 == 0 ? 0 : p[i];
                a.Swap(i, j);

                yield return a;

                p[i]++;
                i = 1;
            }
            else
            {
                p[i] = 0;
                i++;
            }
        }
    }
}
