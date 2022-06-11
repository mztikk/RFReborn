using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class StringToByteSizeTests
    {
        [DataTestMethod]
        [DataRow("50kb", 50000)]
        [DataRow("50 kb", 50000)]
        [DataRow("50", 50)]
        [DataRow("2", 2)]
        [DataRow("13mb", 13000000)]
        [DataRow(" 13mb", 13000000)]
        [DataRow("85gb", 85000000000)]
        [DataRow("98tb", 98000000000000)]
        public void StringToByteSize(string input, long output)
        {
            Assert.AreEqual(output, StringR.StringToByteSize(input));

            Assert.IsTrue(StringR.TryStringToByteSize(input, out long result));
            Assert.AreEqual(output, result);
        }

        [DataTestMethod]
        [DataRow("12abc")]
        [DataRow("test")]
        public void StringToByteSizeNotFormattableAsNumber(string input)
        {
            Assert.ThrowsException<FormatException>(() => StringR.StringToByteSize(input));

            Assert.IsFalse(StringR.TryStringToByteSize(input, out long _));
        }

        [DataTestMethod]
        [DataRow("1111111111111111111111111111111111111111111")]
        [DataRow("12345678910111213141516171819")]
        public void StringToByteSizeNumberOverflow(string input)
        {
            Assert.ThrowsException<OverflowException>(() => StringR.StringToByteSize(input));

            Assert.IsFalse(StringR.TryStringToByteSize(input, out long _));
        }

        [TestMethod]
        public void NullInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringR.StringToByteSize(null));
            Assert.IsFalse(StringR.TryStringToByteSize(null, out long _));
        }
    }
}
