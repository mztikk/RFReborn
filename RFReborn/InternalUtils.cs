using RFReborn.Random;

namespace RFReborn
{
    internal static class InternalUtils
    {
        internal static readonly CryptoRandom s_rng;

        static InternalUtils()
        {
            s_rng = new CryptoRandom();
        }
    }
}
