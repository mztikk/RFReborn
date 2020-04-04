using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;
using RFReborn.Pairs;

namespace RFReborn.Tests.OpsTests
{
    [TestClass]
    public class MemSwapTests
    {
        private const int Seed = 1166782782;

        private List<Pair<byte[]>> _testCases;

        [TestInitialize]
        public void Setup()
        {
            Random random = new Random(Seed);

            List<Pair<byte[]>> generatedCases = new List<Pair<byte[]>>();

            int[] sizes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 54, 1337 };

            foreach (int size in sizes)
            {
                byte[] t1 = new byte[size];
                byte[] t2 = new byte[size];
                random.NextBytes(t1);
                random.NextBytes(t2);

                generatedCases.Add(new Pair<byte[]>(t1, t2));
            }

            _testCases = generatedCases;
        }

        [TestMethod]
        public void FullSwap()
        {
            foreach (Pair<byte[]> testCase in _testCases)
            {
                Assert.AreEqual(testCase.Left.Length, testCase.Right.Length);

                byte[] temp = new byte[testCase.Left.Length];
                Array.Copy(testCase.Left, temp, temp.Length);

                Assert.IsFalse(FastCompare.Equals(testCase.Left, testCase.Right));
                Assert.IsTrue(FastCompare.Equals(testCase.Left, temp));

                Swap(testCase.Left, testCase.Right);

                Assert.IsFalse(FastCompare.Equals(testCase.Left, testCase.Right));
                Assert.IsTrue(FastCompare.Equals(temp, testCase.Right));
            }
        }

        [TestMethod]
        public void HalfSwap()
        {
            foreach (Pair<byte[]> testCase in _testCases)
            {
                // only take testcases with even length for easier splitting
                if (testCase.Left.Length % 2 != 0)
                {
                    continue;
                }

                Assert.AreEqual(testCase.Left.Length, testCase.Right.Length);

                int len = testCase.Left.Length / 2;

                byte[] temp = new byte[testCase.Left.Length];
                Array.Copy(testCase.Left, temp, temp.Length);

                Assert.IsFalse(FastCompare.Equals(testCase.Left, testCase.Right));
                Assert.IsTrue(FastCompare.Equals(testCase.Left, temp));

                Swap(testCase.Left, testCase.Right, len);

                Assert.IsFalse(FastCompare.Equals(testCase.Left, testCase.Right, len));
                Assert.IsTrue(FastCompare.Equals(temp, testCase.Right, len));

                // last half has to be the same still since we only swapped up to first half
                Assert.IsTrue(FastCompare.Equals(temp.Skip(len).ToArray(), testCase.Left.Skip(len).ToArray()));
            }
        }

        private static unsafe void Swap(byte[] left, byte[] right) => Swap(left, right, left.Length);

        private static unsafe void Swap(byte[] left, byte[] right, int len)
        {
            fixed (byte* leftP = left, rightP = right)
            {
                Ops.MemSwap(leftP, rightP, len);
            }
        }
    }
}
