using System.IO;
using RFReborn.RandomR;

namespace RFReborn.Tests
{
    internal static class InternalUtils
    {
        internal static readonly CryptoRandom s_rng = new();

        internal static readonly System.Random s_random = new();

        internal static MemoryStream GetStream(this byte[] array) => new(array);
    }
}
