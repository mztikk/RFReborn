using System.Runtime.CompilerServices;

namespace RFReborn
{
    /// <summary>
    /// Offers methods to compare data.
    /// </summary>
    public static unsafe class FastCompare
    {
        /// <summary>
        /// Checks if two strings are equal by comparing their byte representation in memory.
        /// </summary>
        /// <param name="left">First string to compare.</param>
        /// <param name="right">Second string to compare.</param>
        /// <returns>TRUE if equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string left, string right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            fixed (void* lp = left, rp = right)
            {
                return Equals(lp, rp, left.Length * sizeof(char));
            }
        }

        /// <summary>
        /// Checks if the specified arrays are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First array to compare.</param>
        /// <param name="right">Second array to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T[] left, T[] right) where T : unmanaged
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            fixed (void* lp = left, rp = right)
            {
                return Equals(lp, rp, left.Length * sizeof(T));
            }
        }

        /// <summary>
        /// Compares the specified number of bytes in memory at <paramref name="left"/> and <paramref name="right"/>.
        /// </summary>
        /// <param name="left">Pointer to memory to compare.</param>
        /// <param name="right">Pointer to memory to compare.</param>
        /// <param name="len">Number of bytes to compare.</param>
        /// <returns>TRUE if all bytes are equal in value, FALSE otherwise.</returns>
        public static bool Equals(void* left, void* right, long len)
        {
            byte* pl = (byte*)left;
            byte* pr = (byte*)right;
            byte* bEnd = pl + len;

            while (pl <= bEnd - 8)
            {
                if (*(ulong*)pl != *(ulong*)pr)
                {
                    return false;
                }

                pl += 8;
                pr += 8;
            }

            while (pl <= bEnd - 4)
            {
                if (*(uint*)pl != *(uint*)pr)
                {
                    return false;
                }

                pl += 4;
                pr += 4;
            }

            while (pl < bEnd)
            {
                if (*pl != *pr)
                {
                    return false;
                }

                pl++;
                pr++;
            }

            return true;
        }
    }
}
