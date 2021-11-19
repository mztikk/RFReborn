using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
    public static void Swap<T>(ref T m1, ref T m2)
    {
        T tmp = m1;
        m1 = m2;
        m2 = tmp;
    }

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
            ulong temp = *(ulong*)pl;
            *(ulong*)pl = *(ulong*)pr;
            *(ulong*)pr = temp;

            pl += 8;
            pr += 8;
        }

        while (pl <= bEnd - 4)
        {
            uint temp = *(uint*)pl;
            *(uint*)pl = *(uint*)pr;
            *(uint*)pr = temp;

            pl += 4;
            pr += 4;
        }

        while (pl < bEnd)
        {
            byte temp = *pl;
            *pl = *pr;
            *pr = temp;

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

    /// <summary>
    /// Generates strings based on the specified charset and tests them against the <paramref name="checkFunc"/> until it returns true and returns the matching string.
    /// </summary>
    /// <param name="charset">Characterset to be used when generating strings.</param>
    /// <param name="checkFunc">Function to for the generated string to be checked against.</param>
    /// <param name="startLength">Minimum/Starting length of the generated strings.</param>
    /// <param name="maxLength">Maximum length of the generated strings.</param>
    /// <returns>The string that matched the <paramref name="checkFunc"/> or <see cref="string.Empty"/> otherwise.</returns>
    [Obsolete]
    public static string BruteForce(string charset, Func<string, bool> checkFunc, int startLength = 1, int maxLength = 6) => BruteForce(charset.ToCharArray(), checkFunc, startLength, maxLength);

    /// <summary>
    /// Generates strings based on the specified charset and tests them against the <paramref name="checkFunc"/> until it returns true and returns the matching string.
    /// </summary>
    /// <param name="charset">Characterset to be used when generating strings.</param>
    /// <param name="checkFunc">Function to for the generated string to be checked against.</param>
    /// <param name="startLength">Minimum/Starting length of the generated strings.</param>
    /// <param name="maxLength">Maximum length of the generated strings.</param>
    /// <returns>The string that matched the <paramref name="checkFunc"/> or <see cref="string.Empty"/> otherwise.</returns>
    [Obsolete]
    public static unsafe string BruteForce(IList<char> charset, Func<string, bool> checkFunc, int startLength = 1, int maxLength = 6)
    {
        string found = string.Empty;
        int charsetLength = charset.Count;
        const long startw = 0;
        long[] d = new long[maxLength + 1];
        Dictionary<int, string> set = new(Environment.ProcessorCount);
        for (int length = startLength; length < maxLength; length++)
        {
            long endw = (long)Math.Pow(charset.Count, length);

            for (int i = length; i >= 0; i--)
            {
                d[i] = (long)Math.Pow(charsetLength, i);
            }

            Parallel.For(startw, endw, (ind, loopState) =>
            {
                int id = Environment.CurrentManagedThreadId;
                if (!set.ContainsKey(id) || !set.TryGetValue(id, out string str))
                {
                    str = new string(' ', length);
                    set.Add(id, str);
                }

                long mw = ind;
                fixed (char* strp = str)
                {
                    for (int i = length; i >= 0; i--)
                    {
                        int w = (int)(mw / d[i]);

                        if (i == length)
                        {
                            if (w != 0)
                            {
                                strp[i] = charset[w];
                            }
                        }
                        else
                        {
                            strp[i] = charset[w];
                        }

                        mw -= w * d[i];
                    }

                    if (checkFunc(str))
                    {
                        found = str;
                        loopState.Break();
                    }
                }
            });

            set.Clear();
        }

        return found;
    }
}
