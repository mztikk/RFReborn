using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
using RFReborn.RandomR;

namespace RFReborn.Tests.RandomExtensionsTests
{
    [TestClass]
    public class NextStringTests
    {
        [DataTestMethod]
        [DataRow("ABCDEF", 8)]
        [DataRow("ABCDEF", 20)]
        public void ContainsOnlyCharset(string charset, int len)
        {
            Random rnd = new CryptoRandom();
            foreach (char c in rnd.NextString(charset, len))
            {
                Assert.IsTrue(charset.Contains(c));
            }
        }
    }
}
