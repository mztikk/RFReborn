using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
using RFReborn.Pairs;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class ArrayEqualsTests
    {
        #region Byte Tests
        private readonly List<Pair<byte[]>> _equalBytePairs = new List<Pair<byte[]>>() {
            { new byte[] { 1, 5, 6, 123, 255 },new byte[] { 1, 5, 6, 123, 255 }},
            { new byte[] { 65, 35, 76, 23, 255 },new byte[] { 65, 35, 76, 23, 255 }},
            { new byte[] { 00, 00, 00, 00, 00 },new byte[] { 00, 00, 00, 00, 00 }}
        };

        private readonly List<Pair<byte[]>> _diffBytePairs = new List<Pair<byte[]>>() {
            { new byte[] { 65, 35, 76, 23, 255 },new byte[] { 65, 35, 76, 23 }},
            { new byte[] { 65, 35, 76, 23 },new byte[] { 65, 35, 76, 23, 255 }},
            { new byte[] { 65, 35, 76, 23, 255 },new byte[] { 1, 5, 6, 123, 255 }},
            { new byte[] { 00, 00, 00, 00, 00 },new byte[] { 00, 00, 00, 00, 01 }}
        };

        [TestMethod]
        public void ByteArrayEquals()
        {
            foreach (Pair<byte[]> pair in _equalBytePairs)
            {
                AssertEquals(pair.Left, pair.Right);
            }
            foreach (Pair<byte[]> pair in _diffBytePairs)
            {
                AssertDiff(pair.Left, pair.Right);
            }
        }
        #endregion Byte Tests

        #region Short Tests
        private readonly List<Pair<short[]>> _equalShortPairs = new List<Pair<short[]>>() {
            { new short[] { 1, 5, 6, 123, 255 },new short[] { 1, 5, 6, 123, 255 }},
            { new short[] { 65, 35, 76, 23, 255 },new short[] { 65, 35, 76, 23, 255 }},
            { new short[] { 00, 00, 00, 00, 00 },new short[] { 00, 00, 00, 00, 00 }}
        };

        private readonly List<Pair<short[]>> _diffShortPairs = new List<Pair<short[]>>() {
            { new short[] { 65, 35, 76, 23, 255 },new short[] { 65, 35, 76, 23 }},
            { new short[] { 65, 35, 76, 23 },new short[] { 65, 35, 76, 23, 255 }},
            { new short[] { 65, 35, 76, 23, 255 },new short[] { 1, 5, 6, 123, 255 }},
            { new short[] { 00, 00, 00, 00, 00 },new short[] { 00, 00, 00, 00, 01 }}
        };

        [TestMethod]
        public void ShortArrayEquals()
        {
            foreach (Pair<short[]> pair in _equalShortPairs)
            {
                AssertEquals(pair.Left, pair.Right);
            }
            foreach (Pair<short[]> pair in _diffShortPairs)
            {
                AssertDiff(pair.Left, pair.Right);
            }
        }
        #endregion Short Tests

        #region Int Tests
        private readonly List<Pair<int[]>> _equalIntPairs = new List<Pair<int[]>>() {
            { new int[] { 1, 5, 6, 123, 255 },new int[] { 1, 5, 6, 123, 255 }},
            { new int[] { 65, 35, 76, 23, 255 },new int[] { 65, 35, 76, 23, 255 }},
            { new int[] { 00, 00, 00, 00, 00 },new int[] { 00, 00, 00, 00, 00 }}
        };

        private readonly List<Pair<int[]>> _diffIntPairs = new List<Pair<int[]>>() {
            { new int[] { 65, 35, 76, 23, 255 },new int[] { 65, 35, 76, 23 }},
            { new int[] { 65, 35, 76, 23 },new int[] { 65, 35, 76, 23, 255 }},
            { new int[] { 65, 35, 76, 23, 255 },new int[] { 1, 5, 6, 123, 255 }},
            { new int[] { 00, 00, 00, 00, 00 },new int[] { 00, 00, 00, 00, 01 }}
        };

        [TestMethod]
        public void IntArrayEquals()
        {
            foreach (Pair<int[]> pair in _equalIntPairs)
            {
                AssertEquals(pair.Left, pair.Right);
            }
            foreach (Pair<int[]> pair in _diffIntPairs)
            {
                AssertDiff(pair.Left, pair.Right);
            }
        }
        #endregion Int Tests

        private void AssertEquals<T>(T[] left, T[] right) where T : unmanaged
        {
            Assert.IsTrue(FastCompare.Equals(left, right));
        }

        private void AssertDiff<T>(T[] left, T[] right) where T : unmanaged
        {
            Assert.IsFalse(FastCompare.Equals(left, right));
        }
    }
}
