using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Hashing;

namespace RFReborn.Tests.HashingTests
{
    [TestClass]
    public class xxHashTests
    {
        [DataTestMethod]
        [DataRow("The quick brown fox jumps over the lazy dog", 0xe85ea4de)]
        public void ASCIIStringxxHash(string input, uint output) => Assert.AreEqual(output, xxHash.Hash(Encoding.ASCII.GetBytes(input)));

        [DataRow("The quick brown fox jumps over the lazy dog", 1234, 0xf7580370)]
        public void ASCIIStringSeededxxHash(string input, uint seed, uint output) => Assert.AreEqual(output, xxHash.Hash(Encoding.ASCII.GetBytes(input), seed));

        [DataTestMethod]
        [DataRow("The quick brown fox jumps over the lazy dog", 2607235377)]
        public void UnicodeStringxxHash(string input, uint output) => Assert.AreEqual(output, xxHash.Hash(input));
    }
}
