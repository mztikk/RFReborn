namespace RFReborn.Hashing;

// http://cyan4973.github.io/xxHash/
/// <summary>
/// xxHash is an extremely fast non-cryptographic hash algorithm, working at speeds close to RAM limits. It is proposed in two flavors, 32 and 64 bits.
/// </summary>
public static unsafe class xxHash
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
        void* p = &input;
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
        byte* p = (byte*)input;
        byte* bEnd = p + len;
        uint h32;

        if (len >= 16)
        {
            byte* limit = bEnd - 16;

            uint v1 = seed + PRIME32_1 + PRIME32_2;
            uint v2 = seed + PRIME32_2;
            uint v3 = seed + 0;
            uint v4 = seed - PRIME32_1;

            do
            {
                v1 = rotl13(v1 + ((*(uint*)p) * PRIME32_2)) * PRIME32_1;
                v2 = rotl13(v2 + ((*(uint*)(p + 4)) * PRIME32_2)) * PRIME32_1;
                v3 = rotl13(v3 + ((*(uint*)(p + 8)) * PRIME32_2)) * PRIME32_1;
                v4 = rotl13(v4 + ((*(uint*)(p + 12)) * PRIME32_2)) * PRIME32_1;
                p += 16;

            } while (p <= limit);

            h32 = rotl1(v1) + rotl7(v2) + rotl12(v3) + rotl18(v4);
        }
        else
        {
            h32 = seed + PRIME32_5;
        }

        h32 += (uint)len;

        while (p + 4 <= bEnd)
        {
            h32 = rotl17(h32 + ((*(uint*)p) * PRIME32_3)) * PRIME32_4;
            p += 4;
        }
        while (p < bEnd)
        {
            h32 = rotl11(h32 + ((*p) * PRIME32_5)) * PRIME32_1;
            p++;
        }

        return Avalanche(h32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl1(uint x) => (x << 1) | (x >> 31);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl7(uint x) => (x << 7) | (x >> 25);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl11(uint x) => (x << 11) | (x >> 21);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl12(uint x) => (x << 12) | (x >> 20);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl13(uint x) => (x << 13) | (x >> 19);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl17(uint x) => (x << 17) | (x >> 15);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint rotl18(uint x) => (x << 18) | (x >> 14);

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
