using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Pairs;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class StringEqualsTests
    {
        private readonly List<Pair<string>> _equals = new List<Pair<string>>() { { "test", "test" }, { "!%/$%@üpäüö", "!%/$%@üpäüö" }, { @"

", @"

" } };

        private readonly List<Pair<string>> _diff = new List<Pair<string>>() { { "test", "test1" }, { @"

", @"
" } };

        [TestMethod]
        public void StringEquals()
        {
            foreach (Pair<string> item in _equals)
            {
                AssertEquals(item.Left, item.Right);
            }

            foreach (Pair<string> item in _diff)
            {
                AssertDiff(item.Left, item.Right);
            }
        }

        private void AssertEquals(string left, string right)
        {
            Assert.IsTrue(FastCompare.Equals(left, right));
        }

        private void AssertDiff(string left, string right)
        {
            Assert.IsFalse(FastCompare.Equals(left, right));
        }
    }
}
