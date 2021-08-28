using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.DictionaryExtensionsTests
{
    [TestClass]
    public class ContainsKeyTests
    {
        [TestMethod]
        public void ContainsKey()
        {
            Dictionary<string, int> dict = new()
            {
                ["0"] = 0,
                ["1"] = 1,
                ["2"] = 2,
                ["THREE"] = 3,
            };

            Assert.IsTrue(dict.ContainsKey("1", StringComparison.Ordinal, out string _));
            Assert.IsTrue(dict.ContainsKey("three", StringComparison.OrdinalIgnoreCase, out string key));
            Assert.AreEqual("THREE", key);
            Assert.IsFalse(dict.ContainsKey("three", StringComparison.Ordinal, out string _));
            Assert.IsFalse(dict.ContainsKey("test", StringComparison.Ordinal, out string _));
        }
    }
}
