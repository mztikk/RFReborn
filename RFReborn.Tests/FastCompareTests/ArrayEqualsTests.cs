using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;
using RFReborn.Pairs;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class ArrayEqualsTests
    {
        #region Byte Tests
        private readonly List<Pair<byte[]>> _equalBytePairs = new()
        {
            { new byte[] { 1, 5, 6, 123, 255 }, new byte[] { 1, 5, 6, 123, 255 } },
            { new byte[] { 65, 35, 76, 23, 255 }, new byte[] { 65, 35, 76, 23, 255 } },
            { new byte[] { 00, 00, 00, 00, 00 }, new byte[] { 00, 00, 00, 00, 00 } },
        };

        private readonly List<Pair<byte[]>> _diffBytePairs = new()
        {
            { new byte[] { 65, 35, 76, 23, 255 }, new byte[] { 65, 35, 76, 23 } },
            { new byte[] { 65, 35, 76, 23 }, new byte[] { 65, 35, 76, 23, 255 } },
            { new byte[] { 65, 35, 76, 23, 255 }, new byte[] { 1, 5, 6, 123, 255 } },
            { new byte[] { 00, 00, 00, 00, 00 }, new byte[] { 00, 00, 00, 00, 01 } },
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
        private readonly List<Pair<short[]>> _equalShortPairs = new()
        {
            { new short[] { 1, 5, 6, 123, 255 }, new short[] { 1, 5, 6, 123, 255 } },
            { new short[] { 65, 35, 76, 23, 255 }, new short[] { 65, 35, 76, 23, 255 } },
            { new short[] { 00, 00, 00, 00, 00 }, new short[] { 00, 00, 00, 00, 00 } }
        };

        private readonly List<Pair<short[]>> _diffShortPairs = new()
        {
            { new short[] { 65, 35, 76, 23, 255 }, new short[] { 65, 35, 76, 23 } },
            { new short[] { 65, 35, 76, 23 }, new short[] { 65, 35, 76, 23, 255 } },
            { new short[] { 65, 35, 76, 23, 255 }, new short[] { 1, 5, 6, 123, 255 } },
            { new short[] { 00, 00, 00, 00, 00 }, new short[] { 00, 00, 00, 00, 01 } }
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
        private readonly List<Pair<int[]>> _equalIntPairs = new()
        {
            { new int[] { 1, 5, 6, 123, 255 }, new int[] { 1, 5, 6, 123, 255 } },
            { new int[] { 65, 35, 76, 23, 255 }, new int[] { 65, 35, 76, 23, 255 } },
            { new int[] { 00, 00, 00, 00, 00 }, new int[] { 00, 00, 00, 00, 00 } }
        };

        private readonly List<Pair<int[]>> _diffIntPairs = new()
        {
            { new int[] { 65, 35, 76, 23, 255 }, new int[] { 65, 35, 76, 23 } },
            { new int[] { 65, 35, 76, 23 }, new int[] { 65, 35, 76, 23, 255 } },
            { new int[] { 65, 35, 76, 23, 255 }, new int[] { 1, 5, 6, 123, 255 } },
            { new int[] { 00, 00, 00, 00, 00 }, new int[] { 00, 00, 00, 00, 01 } }
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

        [TestMethod]
        public void NullArray()
        {
            byte[] b1 = null;
            byte[] b2 = null;
            byte[] b3 = new byte[] { 0xFF };
            // both are null should be true since theyre equal
            Assert.IsTrue(FastCompare.Equals(b1, b2));
            Assert.IsTrue(FastCompare.Equals(b1, b2, 15));

            // one is null should be false
            Assert.IsFalse(FastCompare.Equals(b1, b3));
            Assert.IsFalse(FastCompare.Equals(b1, b3, 15));
            Assert.IsFalse(FastCompare.Equals(b3, b1));
            Assert.IsFalse(FastCompare.Equals(b3, b1, 15));
        }

        [TestMethod]
        public void DifferentLengthArray()
        {
            byte[] b1 = new byte[] { 0xFF };
            byte[] b2 = new byte[] { 0xFF, 0xAE };

            Assert.IsFalse(FastCompare.Equals(b1, b2));
            Assert.IsFalse(FastCompare.Equals(b2, b1));
            Assert.IsFalse(FastCompare.Equals(b1, b2, len: 2));
            Assert.IsFalse(FastCompare.Equals(b2, b1, len: 2));
            Assert.IsFalse(FastCompare.Equals(b1, b2, len: 10));
            Assert.IsFalse(FastCompare.Equals(b2, b1, len: 10));

            Assert.IsTrue(FastCompare.Equals(b1, b2, len: 1));
            Assert.IsTrue(FastCompare.Equals(b2, b1, len: 1));
        }

        private static void AssertEquals<T>(T[] left, T[] right) where T : unmanaged
        {
            Assert.IsTrue(FastCompare.Equals(left, right));
            Assert.IsTrue(FastCompare.Equals(left, right, left.Length));
            Assert.IsTrue(FastCompare.Equals(left.AsSpan(), right.AsSpan()));
            Assert.IsTrue(FastCompare.Equals(new ReadOnlySpan<T>(left), new ReadOnlySpan<T>(right)));
            Assert.IsTrue(FastCompare.Equals(left.AsSpan(), new ReadOnlySpan<T>(right)));
        }

        private static void AssertDiff<T>(T[] left, T[] right) where T : unmanaged
        {
            Assert.IsFalse(FastCompare.Equals(left, right));
            Assert.IsFalse(FastCompare.Equals(left.AsSpan(), right.AsSpan()));
            Assert.IsFalse(FastCompare.Equals(new ReadOnlySpan<T>(left), new ReadOnlySpan<T>(right)));
            Assert.IsFalse(FastCompare.Equals(left.AsSpan(), new ReadOnlySpan<T>(right)));
        }
    }
}
