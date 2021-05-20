using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.IEnumerableExtensionsTests
{
    [TestClass]
    public class AnyTests
    {
        [TestMethod]
        public void OneItemInArrayIsTrue()
        {
            int[] test = new int[] { 1 };
            Assert.IsTrue(test.Any());
        }

        [TestMethod]
        public void OneItemInIEnumerableIsTrue()
        {
            static IEnumerable<int> test()
            {
                yield return 0;
            }

            Assert.IsTrue(test().Any());
        }

        [TestMethod]
        public void ItemInIEnumerableMatchesSelector()
        {
            static IEnumerable<int> test()
            {
                yield return 0;
            }

            Assert.IsTrue(test().Any(x => x == 0));
        }

        [TestMethod]
        public void ItemInIEnumerableDoesntMatchSelector()
        {
            static IEnumerable<int> test()
            {
                yield return 0;
            }

            Assert.IsFalse(test().Any(x => x == 1));
        }

        [TestMethod]
        public void EmptyArrayIsFalse() => Assert.IsFalse(Array.Empty<bool>().Any());

        [TestMethod]
        public void EmptyIEnumerableIsFalse()
        {
            static IEnumerable<int> test()
            {
                yield break;
            }

            Assert.IsFalse(test().Any());
        }
    }
}
