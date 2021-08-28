using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.ArithmeticMeanTests
{
    [TestClass]
    public class ArithmeticMeanSbyteTests
    {
        private readonly List<(sbyte[] array, double mean)> _tests = new() {
            (new sbyte[] { 1, 2 }, 1.5),
            (new sbyte[] { 1, 2, 3, 4, 5 }, 3),
            (new sbyte[] { 1, 0, 5, 50, 120, 13 }, 31.5),
        };

        [TestMethod]
        public void ArithmeticMeanSbyte()
        {
            foreach ((sbyte[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(sbyte[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanByteTests
    {
        private readonly List<(byte[] array, double mean)> _tests = new() {
            (new byte[] { 1, 2 }, 1.5),
            (new byte[] { 1, 2, 3, 4, 5 }, 3),
            (new byte[] { 1, 0, 5, 50, 120, 13 }, 31.5),
        };

        [TestMethod]
        public void ArithmeticMeanByte()
        {
            foreach ((byte[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(byte[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanShortTests
    {
        private readonly List<(short[] array, double mean)> _tests = new() {
            (new short[] { 1, 2 }, 1.5),
            (new short[] { 1, 2, 3, 4, 5 }, 3),
            (new short[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new short[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanShort()
        {
            foreach ((short[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(short[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanUshortTests
    {
        private readonly List<(ushort[] array, double mean)> _tests = new() {
            (new ushort[] { 1, 2 }, 1.5),
            (new ushort[] { 1, 2, 3, 4, 5 }, 3),
            (new ushort[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new ushort[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanUshort()
        {
            foreach ((ushort[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(ushort[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanIntTests
    {
        private readonly List<(int[] array, double mean)> _tests = new() {
            (new int[] { 1, 2 }, 1.5),
            (new int[] { 1, 2, 3, 4, 5 }, 3),
            (new int[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new int[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanInt()
        {
            foreach ((int[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(int[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanUintTests
    {
        private readonly List<(uint[] array, double mean)> _tests = new() {
            (new uint[] { 1, 2 }, 1.5),
            (new uint[] { 1, 2, 3, 4, 5 }, 3),
            (new uint[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new uint[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanUint()
        {
            foreach ((uint[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(uint[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanLongTests
    {
        private readonly List<(long[] array, double mean)> _tests = new() {
            (new long[] { 1, 2 }, 1.5),
            (new long[] { 1, 2, 3, 4, 5 }, 3),
            (new long[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new long[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanLong()
        {
            foreach ((long[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(long[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanUlongTests
    {
        private readonly List<(ulong[] array, double mean)> _tests = new() {
            (new ulong[] { 1, 2 }, 1.5),
            (new ulong[] { 1, 2, 3, 4, 5 }, 3),
            (new ulong[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new ulong[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanUlong()
        {
            foreach ((ulong[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(ulong[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanFloatTests
    {
        private readonly List<(float[] array, double mean)> _tests = new() {
            (new float[] { 1, 2 }, 1.5),
            (new float[] { 1, 2, 3, 4, 5 }, 3),
            (new float[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new float[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanFloat()
        {
            foreach ((float[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(float[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }

    [TestClass]
    public class ArithmeticMeanDoubleTests
    {
        private readonly List<(double[] array, double mean)> _tests = new() {
            (new double[] { 1, 2 }, 1.5),
            (new double[] { 1, 2, 3, 4, 5 }, 3),
            (new double[] { 1, 0, 5, 50, 120, 13 }, 31.5),
            (new double[] { 100, 5000, 1337, 50, 120, 13 }, 1103.3333333333333),
        };

        [TestMethod]
        public void ArithmeticMeanDouble()
        {
            foreach ((double[] array, double mean) in _tests)
            {
                AssertIsMean(array, mean);
            }
        }

        private static void AssertIsMean(double[] array, double mean) => Assert.AreEqual(mean, array.Mean());
    }
}

