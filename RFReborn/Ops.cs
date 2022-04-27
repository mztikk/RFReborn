namespace RFReborn;

/// <summary>
/// Provides general operations.
/// </summary>
public static class Ops
{
    /// <summary>
    /// Swaps two variables.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="m1">First value.</param>
    /// <param name="m2">Second value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<T>(ref T m1, ref T m2) => (m2, m1) = (m1, m2);

    /// <summary>
    /// Swaps <paramref name="len"/> number of bytes at the specified pointers.
    /// </summary>
    /// <param name="m1">First pointer.</param>
    /// <param name="m2">Second pointer.</param>
    /// <param name="len">Number of bytes to swap.</param>
    public static unsafe void MemSwap(void* m1, void* m2, int len)
    {
        byte* pl = (byte*)m1;
        byte* pr = (byte*)m2;
        byte* bEnd = pl + len;

        while (pl <= bEnd - 8)
        {
            (*(ulong*)pr, *(ulong*)pl) = (*(ulong*)pl, *(ulong*)pr);
            pl += 8;
            pr += 8;
        }

        while (pl <= bEnd - 4)
        {
            (*(uint*)pr, *(uint*)pl) = (*(uint*)pl, *(uint*)pr);
            pl += 4;
            pr += 4;
        }

        while (pl < bEnd)
        {
            (*pr, *pl) = (*pl, *pr);
            pl++;
            pr++;
        }
    }

    /// <summary>
    /// Merges all given <see cref="IList{T}"/> into one and returns it.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="listArray">All <see cref="IList{T}"/> to merge.</param>
    /// <returns>The merged <see cref="IList{T}"/>.</returns>
    public static IList<T> Merge<T>(params IList<T>[] listArray) => Merge(enumList: listArray);

    /// <summary>
    /// Merges all given <see cref="IList{T}"/> into one and returns it.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="enumList"><see cref="IEnumerable{T}"/> of all <see cref="IList{T}"/> to merge.</param>
    /// <returns>The merged <see cref="IList{T}"/>.</returns>
    public static IList<T> Merge<T>(IEnumerable<IList<T>> enumList)
    {
        int newSize = 0;
        foreach (IList<T> list in enumList)
        {
            newSize += list.Count;
        }

        T[] rtn = new T[newSize];
        int lastIndex = 0;
        foreach (IList<T> list in enumList)
        {
            list.CopyTo(rtn, lastIndex);
            lastIndex += list.Count;
        }

        return rtn;
    }
}
