using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.IEnumerableExtensionsTests
{
    [TestClass]
    public class CountTests
    {
        [TestMethod]
        public void OneItemInArray()
        {
            int[] test = new int[] { 1 };
            Assert.AreEqual(1, test.Count());
        }

        [TestMethod]
        public void EmptyArray() => Assert.AreEqual(0, Array.Empty<int>().Count());

        [TestMethod]
        public void OneItemInIEnumerable()
        {
            static IEnumerable<int> test()
            {
                yield return 1;
            }
            Assert.AreEqual(1, test().Count());
        }

        [TestMethod]
        public void EmptyIEnumerable()
        {
            static IEnumerable<int> test()
            {
                yield break;
            }
            Assert.AreEqual(0, test().Count());
        }
    }
}
