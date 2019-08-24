using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class ByteArrayToHexStringTests
    {
        private readonly List<KeyValuePair<byte[], string>> _tests = new List<KeyValuePair<byte[], string>>() {
            new KeyValuePair<byte[], string>(new byte[]{0x50,0xFF }, "50FF"),
            new KeyValuePair<byte[], string>(new byte[]{0xFD, 0xCE, 0x50, 0x43, 0x24, 0xB7, 0x23, 0x67, 0xB1, 0x1F, 0xE1, 0xB7, 0x63, 0x62, 0x10, 0xEC, 0x00  }, "FDCE504324B72367B11FE1B7636210EC00"),
        };

        [TestMethod]
        public void ByteArrayToHexString()
        {
            foreach (KeyValuePair<byte[], string> pair in _tests)
            {
                AssertEquals(pair.Key, pair.Value);
            }
        }

        private void AssertEquals(byte[] toTest, string expected) => Assert.AreEqual(expected, StringR.ByteArrayToHexString(toTest));
    }
}
