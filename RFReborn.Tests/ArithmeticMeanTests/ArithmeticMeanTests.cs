using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.ArithmeticMeanTests
{
    [TestClass]
    public class ArithmeticMeanSbyteTests
    {
        private readonly List<(sbyte[] array, sbyte mean)> _tests = new List<(sbyte[], sbyte)>() {
            (new sbyte[] { 1, 2, 3, 4, 5 }, 3),
            (new sbyte[] { 1, 0, 5, 50, 120, 13 }, 31),
        };

        [TestMethod]
        public void ArithmeticMeanSbyte()
        {
            foreach ((sbyte[] array, sbyte mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(sbyte[] array, sbyte mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanByteTests
    {
        private readonly List<(byte[] array, byte mean)> _tests = new List<(byte[], byte)>() {
            (new byte[] { 1, 2, 3, 4, 5 }, 3),
            (new byte[] { 1, 0, 5, 50, 120, 13 }, 31),
        };

        [TestMethod]
        public void ArithmeticMeanByte()
        {
            foreach ((byte[] array, byte mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(byte[] array, byte mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanShortTests
    {
        private readonly List<(short[] array, short mean)> _tests = new List<(short[], short)>() {
            (new short[] { 1, 2, 3, 4, 5 }, 3),
            (new short[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new short[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanShort()
        {
            foreach ((short[] array, short mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(short[] array, short mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanUshortTests
    {
        private readonly List<(ushort[] array, ushort mean)> _tests = new List<(ushort[], ushort)>() {
            (new ushort[] { 1, 2, 3, 4, 5 }, 3),
            (new ushort[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new ushort[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanUshort()
        {
            foreach ((ushort[] array, ushort mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(ushort[] array, ushort mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanIntTests
    {
        private readonly List<(int[] array, int mean)> _tests = new List<(int[], int)>() {
            (new int[] { 1, 2, 3, 4, 5 }, 3),
            (new int[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new int[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanInt()
        {
            foreach ((int[] array, int mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(int[] array, int mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanUintTests
    {
        private readonly List<(uint[] array, uint mean)> _tests = new List<(uint[], uint)>() {
            (new uint[] { 1, 2, 3, 4, 5 }, 3),
            (new uint[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new uint[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanUint()
        {
            foreach ((uint[] array, uint mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(uint[] array, uint mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanLongTests
    {
        private readonly List<(long[] array, long mean)> _tests = new List<(long[], long)>() {
            (new long[] { 1, 2, 3, 4, 5 }, 3),
            (new long[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new long[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanLong()
        {
            foreach ((long[] array, long mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(long[] array, long mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }

    [TestClass]
    public class ArithmeticMeanUlongTests
    {
        private readonly List<(ulong[] array, ulong mean)> _tests = new List<(ulong[], ulong)>() {
            (new ulong[] { 1, 2, 3, 4, 5 }, 3),
            (new ulong[] { 1, 0, 5, 50, 120, 13 }, 31),
            (new ulong[] { 100, 5000, 1337, 50, 120, 13 }, 1103),
        };

        [TestMethod]
        public void ArithmeticMeanUlong()
        {
            foreach ((ulong[] array, ulong mean) test in _tests)
            {
                AssertIsMean(test.array, test.mean);
            }
        }

        private void AssertIsMean(ulong[] array, ulong mean)
        {
            Assert.AreEqual(mean, array.Mean());
        }
    }
}

