using RFReborn.Random;

namespace RFReborn.Tests
{
    internal static class InternalUtils
    {
        internal static readonly CryptoRandom s_rng = new CryptoRandom();

        internal static readonly System.Random s_random = new System.Random();
    }
}
