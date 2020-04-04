using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace RFReborn.Comparison
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

            return Equals(left, right, left.Length);
        }

        /// <summary>
        /// Checks if the specified arrays are equal in value up to a specified length.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First array to compare.</param>
        /// <param name="right">Second array to compare.</param>
        /// <param name="len">length to use, can't be bigger than either array</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T[] left, T[] right, long len) where T : unmanaged
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            if (len > left.Length || len > right.Length)
            {
                return false;
            }

            fixed (void* lp = left, rp = right)
            {
                return Equals(lp, rp, len * sizeof(T));
            }
        }

        /// <summary>
        /// Checks if the specified <see cref="Span{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="Span{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="Span{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(Span<T> left, Span<T> right) where T : unmanaged
        {
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
        /// Checks if the specified <see cref="ReadOnlySpan{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right) where T : unmanaged
        {
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
        /// Checks if the specified <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="Span{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(Span<T> left, ReadOnlySpan<T> right) where T : unmanaged
        {
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

        /// <summary>
        /// Compares the bytes in two streams for equality.
        /// </summary>
        /// <param name="left"><see cref="Stream"/> to compare.</param>
        /// <param name="right"><see cref="Stream"/> to compare.</param>
        /// <returns>TRUE if all bytes are equal in value, FALSE otherwise.</returns>
        public static bool Equals(Stream left, Stream right)
        {
            if (left.Length != right.Length || left.Position != right.Position)
            {
                return false;
            }

            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            int bufferSize = (int)Math.Min(wantedBuffersize, left.Length);
            byte[] leftBuffer = new byte[bufferSize];
            byte[] rightBuffer = new byte[bufferSize];

            long numBytesToRead = left.Length;
            while (numBytesToRead > 0)
            {
                int leftRead = left.Read(leftBuffer, 0, bufferSize);
                int rightRead = right.Read(rightBuffer, 0, bufferSize);

                if (leftRead != rightRead)
                {
                    return false;
                }

                if (!Equals(leftBuffer, rightBuffer))
                {
                    return false;
                }

                numBytesToRead -= leftRead;
            }

            return true;
        }

        /// <summary>
        /// Compares the bytes in two streams for inequality.
        /// </summary>
        /// <param name="left"><see cref="Stream"/> to compare.</param>
        /// <param name="right"><see cref="Stream"/> to compare.</param>
        /// <returns>FALSE if all bytes are equal in value, TRUE otherwise.</returns>
        public static bool NotEquals(Stream left, Stream right)
        {
            if (left.Length != right.Length || left.Position != right.Position)
            {
                return true;
            }

            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            int bufferSize = (int)Math.Min(wantedBuffersize, left.Length);
            byte[] leftBuffer = new byte[bufferSize];
            byte[] rightBuffer = new byte[bufferSize];

            long numBytesToRead = left.Length;
            while (numBytesToRead > 0)
            {
                int leftRead = left.Read(leftBuffer, 0, bufferSize);
                int rightRead = right.Read(rightBuffer, 0, bufferSize);

                if (leftRead != rightRead)
                {
                    return true;
                }

                if (!Equals(leftBuffer, rightBuffer))
                {
                    return true;
                }

                numBytesToRead -= leftRead;
            }

            return false;
        }
    }
}
