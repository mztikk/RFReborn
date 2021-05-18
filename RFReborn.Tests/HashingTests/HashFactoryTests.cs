using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Hashing;

namespace RFReborn.Tests.HashingTests
{
    [TestClass]
    public class HashFactoryTests
    {
        [DataTestMethod]
        [DataRow("MD5", "The quick brown fox jumps over the lazy dog", "9E107D9D372BB6826BD81D3542A419D6")]
        [DataRow("SHA1", "The quick brown fox jumps over the lazy dog", "2FD4E1C67A2D28FCED849EE1BB76E7391B93EB12")]
        [DataRow("SHA256", "The quick brown fox jumps over the lazy dog", "D7A8FBB307D7809469CA9ABCB0082E4F8D5651E46D3CDB762D02D0BF37C9E592")]
        [DataRow("SHA384", "The quick brown fox jumps over the lazy dog", "CA737F1014A48F4C0B6DD43CB177B0AFD9E5169367544C494011E3317DBF9A509CB1E5DC1E85A941BBEE3D7F2AFBC9B1")]
        [DataRow("SHA512", "The quick brown fox jumps over the lazy dog", "07E547D9586F6A73F73FBAC0435ED76951218FB7D0C8D788A309D785436BBB642E93A252A954F23912547D1E8A3B5ED6E1BFD7097821233FA0538F3DB854FEE6")]
        public void Utf8StringTests(string hashName, string input, string output)
        {
            Assert.AreEqual(output, HashFactory.Hash(hashName, input));
            Assert.AreEqual(output, HashFactory.Hash(HashFactory.GetHashName(hashName), input));
            Assert.AreEqual(output, HashFactory.Hash(HashFactory.GetHashName(hashName.ToLowerInvariant()), input));
            Assert.AreEqual(output, HashFactory.Hash(HashFactory.GetHashName(hashName.ToUpperInvariant()), input));
        }

        [DataTestMethod]
        [DataRow("not a valid hash method")]
        public void InvalidHashName(string hashName)
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(HashFactory.GetHashName(hashName)));
        }
    }
}
