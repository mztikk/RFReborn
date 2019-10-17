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
            byte[] bytes = new byte[size];
            InternalUtils.s_rng.NextBytes(bytes);
            long rndIndex = InternalUtils.s_rng.Next(0, bytes.Length - signature.Pattern.LongLength);
            Array.Copy(signature.Pattern, 0, bytes, rndIndex, signature.Pattern.LongLength);

            return new AoBTest(bytes, signature);
        }

        public static AoBTest ForSignature(Signature signature, long size, long index)
        {
            byte[] bytes = new byte[size];
            InternalUtils.s_rng.NextBytes(bytes);
            Array.Copy(signature.Pattern, 0, bytes, index, signature.Pattern.LongLength);

            return new AoBTest(bytes, signature);
        }
    }
}
