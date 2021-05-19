



using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
namespace RFReborn.Tests.AddOrIncrementTests
{

    [TestClass]
    public class AddOrIncrementsbyteTests
    {
        [TestMethod]
        public void AddOrIncrementsbyte()
        {
            var key = "test";
            var dict = new Dictionary<string, sbyte>();
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
        public void AddOrIncrementsbyteWithValue(sbyte value)
        {
            var key = "test";
            var dict = new Dictionary<string, sbyte>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((sbyte)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((sbyte)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementbyteTests
    {
        [TestMethod]
        public void AddOrIncrementbyte()
        {
            var key = "test";
            var dict = new Dictionary<string, byte>();
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
        public void AddOrIncrementbyteWithValue(byte value)
        {
            var key = "test";
            var dict = new Dictionary<string, byte>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((byte)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((byte)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementshortTests
    {
        [TestMethod]
        public void AddOrIncrementshort()
        {
            var key = "test";
            var dict = new Dictionary<string, short>();
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
        public void AddOrIncrementshortWithValue(short value)
        {
            var key = "test";
            var dict = new Dictionary<string, short>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((short)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((short)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementushortTests
    {
        [TestMethod]
        public void AddOrIncrementushort()
        {
            var key = "test";
            var dict = new Dictionary<string, ushort>();
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
        public void AddOrIncrementushortWithValue(ushort value)
        {
            var key = "test";
            var dict = new Dictionary<string, ushort>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((ushort)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((ushort)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementintTests
    {
        [TestMethod]
        public void AddOrIncrementint()
        {
            var key = "test";
            var dict = new Dictionary<string, int>();
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
        public void AddOrIncrementintWithValue(int value)
        {
            var key = "test";
            var dict = new Dictionary<string, int>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((int)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((int)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementuintTests
    {
        [TestMethod]
        public void AddOrIncrementuint()
        {
            var key = "test";
            var dict = new Dictionary<string, uint>();
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
        public void AddOrIncrementuintWithValue(uint value)
        {
            var key = "test";
            var dict = new Dictionary<string, uint>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((uint)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((uint)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementlongTests
    {
        [TestMethod]
        public void AddOrIncrementlong()
        {
            var key = "test";
            var dict = new Dictionary<string, long>();
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
        public void AddOrIncrementlongWithValue(long value)
        {
            var key = "test";
            var dict = new Dictionary<string, long>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((long)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((long)(value*3), dict[key]);
        }
    }



    [TestClass]
    public class AddOrIncrementulongTests
    {
        [TestMethod]
        public void AddOrIncrementulong()
        {
            var key = "test";
            var dict = new Dictionary<string, ulong>();
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
        public void AddOrIncrementulongWithValue(ulong value)
        {
            var key = "test";
            var dict = new Dictionary<string, ulong>();
            dict.AddOrIncrement(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((ulong)(value*2), dict[key]);
            dict.AddOrIncrement(key, value);
            Assert.AreEqual((ulong)(value*3), dict[key]);
        }
    }

}
