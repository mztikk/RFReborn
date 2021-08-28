using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class PointerEqualsTests
    {
        private readonly byte[] _byteTest1 = new byte[] { 1, 5, 6, 123, 255 };
        private readonly byte[] _byteTest2 = new byte[] { 65, 35, 76, 23, 255 };
        private readonly byte[] _byteTest3 = new byte[] { 65, 35, 76, 23 };

        private readonly short[] _shortTest1 = new short[] { 1, 5, 6, 123, 754 };
        private readonly short[] _shortTest2 = new short[] { 65, 35, 76, 23, 754 };
        private readonly short[] _shortTest3 = new short[] { 65, 35, 76, 23 };

        private readonly int[] _intTest1 = new int[] { 3456, 54743, 23423, 75642, 234 };
        private readonly int[] _intTest2 = new int[] { 546, 8654, 635, 34764, 0 };
        private readonly int[] _intTest3 = new int[] { 546, 8654, 635, 34764, };

        [TestMethod]
        public void PointerEquals()
        {
            AssertEquals(_byteTest1, _byteTest1);
            AssertEquals(_byteTest2, _byteTest2);
            AssertDiff(_byteTest1, _byteTest2);
            AssertDiff(_byteTest1, _byteTest3);
            AssertDiff(_byteTest2, _byteTest3);

            AssertEquals(_shortTest1, _shortTest1);
            AssertEquals(_shortTest2, _shortTest2);
            AssertDiff(_shortTest1, _shortTest2);
            AssertDiff(_shortTest1, _shortTest3);
            AssertDiff(_shortTest2, _shortTest3);

            AssertEquals(_intTest1, _intTest1);
            AssertEquals(_intTest2, _intTest2);
            AssertDiff(_intTest1, _intTest2);
            AssertDiff(_intTest1, _intTest3);
            AssertDiff(_intTest2, _intTest3);
        }

        private static unsafe bool GetPtrEquals<T>(T[] left, T[] right) where T : unmanaged
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            fixed (void* lp = left, rp = right)
            {
                return FastCompare.Equals(lp, rp, left.Length * sizeof(T));
            }
        }

        private static void AssertEquals<T>(T[] left, T[] right) where T : unmanaged => Assert.IsTrue(GetPtrEquals(left, right));

        private static void AssertDiff<T>(T[] left, T[] right) where T : unmanaged => Assert.IsFalse(GetPtrEquals(left, right));
    }
}
