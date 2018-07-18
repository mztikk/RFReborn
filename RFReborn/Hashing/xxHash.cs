using System.Runtime.CompilerServices;

namespace RFReborn.Hashing
{
    // http://cyan4973.github.io/xxHash/
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S101 // Types should be named in camel case
    /// <summary>
    /// xxHash is an extremely fast non-cryptographic hash algorithm, working at speeds close to RAM limits. It is proposed in two flavors, 32 and 64 bits. 
    /// </summary>
    public static unsafe class xxHash
#pragma warning restore S101 // Types should be named in camel case
#pragma warning restore IDE1006 // Naming Styles
    {
        private const uint PRIME32_1 = 2654435761U;
        private const uint PRIME32_2 = 2246822519U;
        private const uint PRIME32_3 = 3266489917U;
        private const uint PRIME32_4 = 668265263U;
        private const uint PRIME32_5 = 374761393U;

        /// <summary>
        /// Computes the hash for the specified byte array.
        /// </summary>
        /// <param name="input">Byte array to be hashed.</param>
        /// <param name="seed">Each accumulator gets an initial value based on optional seed input. Since the seed is optional, it can be 0.</param>
        /// <returns>The computed hash code.</returns>
        public static uint Hash(byte[] input, uint seed = 0)
        {
            fixed (byte* inputP = input)
            {
                return Hash(inputP, input.Length, seed);
            }
        }

        /// <summary>
        /// Computes the hash value for the specified value.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="input">Value to be hashed.</param>
        /// <param name="seed">Each accumulator gets an initial value based on optional seed input. Since the seed is optional, it can be 0.</param>
        /// <returns>The computed hash code.</returns>
        public static uint Hash<T>(T input, uint seed = 0) where T : unmanaged
        {
            var p = (void*)&input;
            return Hash(p, sizeof(T), seed);
        }

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="input">String to be hashed.</param>
        /// <param name="seed">Each accumulator gets an initial value based on optional seed input. Since the seed is optional, it can be 0.</param>
        /// <returns>The computed hash code.</returns>
        public static uint Hash(string input, uint seed = 0)
        {
            fixed (char* p = input)
            {
                return Hash(p, input.Length * sizeof(char), seed);
            }
        }

        /// <summary>
        /// Computes the hash value for the specified region of memory.
        /// </summary>
        /// <param name="input">Pointer to start of data to be hashed.</param>
        /// <param name="len">Length of data to be hashed in bytes.</param>
        /// <param name="seed">Each accumulator gets an initial value based on optional seed input. Since the seed is optional, it can be 0.</param>
        /// <returns>The computed hash code.</returns>
        public static uint Hash(void* input, int len, uint seed = 0)
        {
            var p = (byte*)input;
            var bEnd = p + len;
            uint h32;

            if (len >= 16)
            {
                var limit = bEnd - 15;

                var v1 = seed + PRIME32_1 + PRIME32_2;
                var v2 = seed + PRIME32_2;
                var v3 = seed + 0;
                var v4 = seed - PRIME32_1;

                do
                {
                    // inlined Round and Get32Bits per hand since it didn't seem to do that even with AggressiveInlining and it has to be inlined for better performance
                    v1 += *((uint*)p) * PRIME32_2;
                    v1 = MathR.RotateLeft(v1, 13);
                    v1 *= PRIME32_1;
                    p += 4;

                    v2 += *((uint*)p) * PRIME32_2;
                    v2 = MathR.RotateLeft(v2, 13);
                    v2 *= PRIME32_1;
                    p += 4;

                    v3 += *((uint*)p) * PRIME32_2;
                    v3 = MathR.RotateLeft(v3, 13);
                    v3 *= PRIME32_1;
                    p += 4;

                    v4 += *((uint*)p) * PRIME32_2;
                    v4 = MathR.RotateLeft(v4, 13);
                    v4 *= PRIME32_1;
                    p += 4;

                } while (p < limit);

                h32 = MathR.RotateLeft(v1, 1) + MathR.RotateLeft(v2, 7) + MathR.RotateLeft(v3, 12) + MathR.RotateLeft(v4, 18);
            }
            else
            {
                h32 = seed + PRIME32_5;
            }

            h32 += (uint)len;

            while (p < bEnd - 4)
            {
                h32 += *((uint*)p) * PRIME32_3;
                h32 = MathR.RotateLeft(h32, 17) * PRIME32_4;
                p += 4;
            }
            while (p < bEnd)
            {
                h32 += *p * PRIME32_5;
                h32 = MathR.RotateLeft(h32, 11) * PRIME32_1;
                p += 1;
            }

            return Avalanche(h32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Avalanche(uint h32)
        {
            h32 ^= h32 >> 15;
            h32 *= PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= PRIME32_3;
            h32 ^= h32 >> 16;
            return h32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round(uint seed, uint input)
        {
            seed += input * PRIME32_2;
            seed = MathR.RotateLeft(seed, 13);
            seed += PRIME32_1;
            return seed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Get32Bits(void* ptr) => *(uint*)ptr;
    }
}
