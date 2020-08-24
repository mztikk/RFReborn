using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;

namespace RFReborn.Tests.AoBTests
{
    [TestClass]
    public class SignatureTests
    {
        private readonly List<StringSignatureTest> _stringSignatureTests = new List<StringSignatureTest> {
            new StringSignatureTest("AE FF ?? 00 FE", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest("AE FF ?F 00 FE", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest("AE FF F? 00 FE", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest("AEFF??00FE", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest("AEF F??00F E", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest(" A E F F ? ? 0 0 F E ", "xx?xx", new byte[] { 0xAE, 0xFF, 0x00, 0x00, 0xFE }),
            new StringSignatureTest("AE FF FF 00 FE", "xxxxx", new byte[] { 0xAE, 0xFF, 0xFF, 0x00, 0xFE }),
            new StringSignatureTest("????????", "????", new byte[] { 0x00, 0x00, 0x00, 0x00 }),
        };

        [TestMethod]
        public void StringSignature()
        {
            foreach (StringSignatureTest test in _stringSignatureTests)
            {
                Assert.AreEqual(test.ExpectedMask, test.Signature.Mask);
                //Assert.IsTrue(FastCompare.Equals(test.ExpectedPattern, test.Signature.Pattern));
                CollectionAssert.AreEqual(test.ExpectedPattern, test.Signature.Pattern);
            }
        }
    }
}
