﻿using RFReborn.AoB;

namespace RFReborn.Tests.AoBTests
{
    public class StringSignatureTest
    {
        public string Sig { get; }
        public string ExpectedMask { get; }
        public byte[] ExpectedPattern { get; }
        public Signature Signature { get; }
        public string ExpectedSig { get; }

        public StringSignatureTest(string sig, string expectedSig, string expectedMask, byte[] expectedPattern)
        {
            Sig = sig;
            ExpectedMask = expectedMask;
            ExpectedPattern = expectedPattern;

            Signature = new Signature(sig);
            ExpectedSig = expectedSig;
        }
    }
}
