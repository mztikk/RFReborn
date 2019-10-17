using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.AoB;

namespace RFReborn.Tests.AoBTests
{
    [TestClass]
    public class AobTests
    {
        private readonly List<AoBTest> _tests = new List<AoBTest>()
        {
            { AoBTestGenerator.ForSignature(new Signature("05010203")) },
            { AoBTestGenerator.ForSignature(new Signature("ff??aeff")) },
            { AoBTestGenerator.ForSignature(new Signature("ff??aeff"), 4048 * 3, 4045) },
        };

        [TestMethod]
        public void SignatureInByteArray()
        {
            foreach (AoBTest test in _tests)
            {
                AssertFound(test.SearchRegion, test.Signature);
            }
        }

        [TestMethod]
        public void SignatureInStream()
        {
            foreach (AoBTest test in _tests)
            {
                AssertFound(test.GetSearchRegionAsStream(), test.Signature);
            }
        }

        private void AssertFound(byte[] searchRegion, Signature signature)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreNotEqual(-1, find);
        }

        private void AssertFound(Stream searchRegion, Signature signature)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreNotEqual(-1, find);
        }
    }
}
