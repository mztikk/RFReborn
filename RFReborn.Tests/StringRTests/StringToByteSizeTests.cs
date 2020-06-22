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
        public void StringToByteSize(string input, long output) => Assert.AreEqual(output, StringR.StringToByteSize(input));

        [DataTestMethod]
        [DataRow("12abc")]
        [DataRow("test")]
        public void StringToByteSizeWrongInput(string input) => Assert.ThrowsException<ArgumentException>(() => StringR.StringToByteSize(input));
    }
}
