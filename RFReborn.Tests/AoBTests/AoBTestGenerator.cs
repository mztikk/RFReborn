using System;
using RFReborn.AoB;
using RFReborn.Extensions;

namespace RFReborn.Tests.AoBTests
{
    internal static class AoBTestGenerator
    {
        public static AoBTest ForSignature(Signature signature)
        {
            long len = signature.Pattern.LongLength;
            // TODO: better way to calculate size
            long size = InternalUtils.s_rng.Next(len, len * 2);
            return ForSignature(signature, size);
        }

        public static AoBTest ForSignature(Signature signature, long size)
        {
            long rndIndex = InternalUtils.s_rng.Next(0, size - signature.Pattern.LongLength);
            return ForSignature(signature, size, rndIndex);
        }

        public static AoBTest ForSignature(Signature signature, long size, long index)
        {
            byte[] bytes = new byte[size];
            InternalUtils.s_rng.NextBytes(bytes);
            Array.Copy(signature.Pattern, 0, bytes, index, signature.Pattern.LongLength);

            return new AoBTest(bytes, signature, index);
        }
    }
}
