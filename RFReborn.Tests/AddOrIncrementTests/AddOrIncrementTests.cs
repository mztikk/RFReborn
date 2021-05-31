using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
namespace RFReborn.Tests.AddOrIncrementTests
{
    [TestClass]
    public class AddOrIncrementSbyteTests
    {
        [TestMethod]
        public void AddOrIncrementSbyte()
        {
            const string key = "test";
            Dictionary<string, sbyte> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((sbyte)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((sbyte)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((sbyte)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((sbyte)5)]
        [DataRow((sbyte)10)]
        public void AddOrIncrementSbyteWithValue(sbyte value)
        {
            const string key = "test";
            Dictionary<string, sbyte> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementByteTests
    {
        [TestMethod]
        public void AddOrIncrementByte()
        {
            const string key = "test";
            Dictionary<string, byte> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((byte)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((byte)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((byte)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((byte)5)]
        [DataRow((byte)10)]
        public void AddOrIncrementByteWithValue(byte value)
        {
            const string key = "test";
            Dictionary<string, byte> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementShortTests
    {
        [TestMethod]
        public void AddOrIncrementShort()
        {
            const string key = "test";
            Dictionary<string, short> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((short)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((short)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((short)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((short)5)]
        [DataRow((short)10)]
        public void AddOrIncrementShortWithValue(short value)
        {
            const string key = "test";
            Dictionary<string, short> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementUshortTests
    {
        [TestMethod]
        public void AddOrIncrementUshort()
        {
            const string key = "test";
            Dictionary<string, ushort> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((ushort)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((ushort)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((ushort)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((ushort)5)]
        [DataRow((ushort)10)]
        public void AddOrIncrementUshortWithValue(ushort value)
        {
            const string key = "test";
            Dictionary<string, ushort> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementIntTests
    {
        [TestMethod]
        public void AddOrIncrementInt()
        {
            const string key = "test";
            Dictionary<string, int> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((int)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((int)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((int)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((int)5)]
        [DataRow((int)10)]
        public void AddOrIncrementIntWithValue(int value)
        {
            const string key = "test";
            Dictionary<string, int> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementUintTests
    {
        [TestMethod]
        public void AddOrIncrementUint()
        {
            const string key = "test";
            Dictionary<string, uint> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((uint)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((uint)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((uint)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((uint)5)]
        [DataRow((uint)10)]
        public void AddOrIncrementUintWithValue(uint value)
        {
            const string key = "test";
            Dictionary<string, uint> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementLongTests
    {
        [TestMethod]
        public void AddOrIncrementLong()
        {
            const string key = "test";
            Dictionary<string, long> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((long)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((long)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((long)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((long)5)]
        [DataRow((long)10)]
        public void AddOrIncrementLongWithValue(long value)
        {
            const string key = "test";
            Dictionary<string, long> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }

    [TestClass]
    public class AddOrIncrementUlongTests
    {
        [TestMethod]
        public void AddOrIncrementUlong()
        {
            const string key = "test";
            Dictionary<string, ulong> dict = new();
            dict.AddOrIncrement(key);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual((ulong)1, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((ulong)2, dict[key]);
            dict.AddOrIncrement(key);
            Assert.AreEqual((ulong)3, dict[key]);
        }

        [DataTestMethod]
        [DataRow((ulong)5)]
        [DataRow((ulong)10)]
        public void AddOrIncrementUlongWithValue(ulong value)
        {
            const string key = "test";
            Dictionary<string, ulong> dict = new();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((value*3), dict[key]);
        }
    }
}
