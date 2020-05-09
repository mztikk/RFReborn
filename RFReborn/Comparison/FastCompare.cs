using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RFReborn.Comparison
{
    /// <summary>
    /// Offers methods to compare data.
    /// </summary>
    public static class FastCompare
    {
        /// <summary>
        /// Checks if two strings are equal by comparing their byte representation in memory.
        /// </summary>
        /// <param name="left">First string to compare.</param>
        /// <param name="right">Second string to compare.</param>
        /// <returns>TRUE if equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals(string left, string right)
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
        public static unsafe bool Equals<T>(T[] left, T[] right, long len) where T : unmanaged
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
        public static unsafe bool Equals<T>(Span<T> left, Span<T> right) where T : unmanaged => Equals(left, right, left.Length);

        /// <summary>
        /// Checks if the specified <see cref="Span{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="Span{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="Span{T}"/> to compare.</param>
        /// <param name="len">length to use, can't be bigger than either array</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals<T>(Span<T> left, Span<T> right, long len) where T : unmanaged
        {
            if (left.Length != right.Length)
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
        /// Checks if the specified <see cref="ReadOnlySpan{T}"/> are equal in value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="left">First <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <param name="right">Second <see cref="ReadOnlySpan{T}"/> to compare.</param>
        /// <returns>TRUE if all values are equal, FALSE otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right) where T : unmanaged
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
        public static unsafe bool Equals<T>(Span<T> left, ReadOnlySpan<T> right) where T : unmanaged
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
        public static unsafe bool Equals(void* left, void* right, long len)
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
            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] leftBuffer = pool.Rent(wantedBuffersize);
            byte[] rightBuffer = pool.Rent(wantedBuffersize);
            Span<byte> leftSpan = leftBuffer.AsSpan();
            Span<byte> rightSpan = rightBuffer.AsSpan();
            try
            {
                int leftRead;
                while ((leftRead = left.Read(leftSpan)) > 0)
                {
                    int rightRead = right.Read(rightSpan);

                    if (leftRead != rightRead)
                    {
                        return false;
                    }

                    if (!Equals(leftSpan, rightSpan, leftRead))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                pool.Return(leftBuffer);
                pool.Return(rightBuffer);
            }
        }

        /// <summary>
        /// Compares the bytes in two streams for equality.
        /// </summary>
        /// <param name="left"><see cref="Stream"/> to compare.</param>
        /// <param name="right"><see cref="Stream"/> to compare.</param>
        /// <returns>TRUE if all bytes are equal in value, FALSE otherwise.</returns>
        public static async Task<bool> EqualsAsync(Stream left, Stream right)
        {
            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            MemoryPool<byte> pool = MemoryPool<byte>.Shared;
            IMemoryOwner<byte> leftOwner = pool.Rent(wantedBuffersize);
            IMemoryOwner<byte> rightOwner = pool.Rent(wantedBuffersize);
            Memory<byte> leftBuffer = leftOwner.Memory;
            Memory<byte> rightBuffer = rightOwner.Memory;
            try
            {
                int leftRead;
                while ((leftRead = await left.ReadAsync(leftBuffer).ConfigureAwait(false)) > 0)
                {
                    int rightRead = await right.ReadAsync(rightBuffer).ConfigureAwait(false);

                    if (leftRead != rightRead)
                    {
                        return false;
                    }

                    if (!Equals(leftBuffer.Span, rightBuffer.Span))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                leftOwner.Dispose();
                rightOwner.Dispose();
            }
        }

        /// <summary>
        /// Compares the bytes in two streams for equality.
        /// </summary>
        /// <param name="left"><see cref="Stream"/> to compare.</param>
        /// <param name="right"><see cref="Stream"/> to compare.</param>
        /// <param name="length">number of bytes to compare</param>
        /// <returns>TRUE if all bytes are equal in value, FALSE otherwise.</returns>
        public static bool Equals(Stream left, Stream right, int length)
        {
            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            int bufferSize = Math.Min(wantedBuffersize, length);
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] leftBuffer = pool.Rent(bufferSize);
            byte[] rightBuffer = pool.Rent(bufferSize);
            try
            {
                while (length > 0)
                {
                    int leftRead = left.Read(leftBuffer, 0, bufferSize);
                    int rightRead = right.Read(rightBuffer, 0, bufferSize);

                    if (leftRead != rightRead)
                    {
                        return false;
                    }

                    if (!Equals(leftBuffer, rightBuffer, leftRead))
                    {
                        return false;
                    }

                    length -= leftRead;
                }

                return true;
            }
            finally
            {
                pool.Return(leftBuffer);
                pool.Return(rightBuffer);
            }
        }

        /// <summary>
        /// Compares the bytes in two streams for inequality.
        /// </summary>
        /// <param name="left"><see cref="Stream"/> to compare.</param>
        /// <param name="right"><see cref="Stream"/> to compare.</param>
        /// <returns>FALSE if all bytes are equal in value, TRUE otherwise.</returns>
        public static bool NotEquals(Stream left, Stream right)
        {
            const int wantedBuffersize = InternalUtils.StreamBufferSize;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] leftBuffer = pool.Rent(wantedBuffersize);
            byte[] rightBuffer = pool.Rent(wantedBuffersize);

            try
            {
                int leftRead;
                while ((leftRead = left.Read(leftBuffer, 0, wantedBuffersize)) > 0)
                {
                    int rightRead = right.Read(rightBuffer, 0, wantedBuffersize);

                    if (leftRead != rightRead)
                    {
                        return true;
                    }

                    if (!Equals(leftBuffer, rightBuffer, leftRead))
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                pool.Return(leftBuffer);
                pool.Return(rightBuffer);
            }
        }
    }
}
