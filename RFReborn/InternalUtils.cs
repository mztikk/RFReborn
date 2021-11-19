using RFReborn.RandomR;

namespace RFReborn;

internal static class InternalUtils
{
    internal static readonly CryptoRandom s_rng = new();
    internal const int StreamBufferSize = 8192;
}
