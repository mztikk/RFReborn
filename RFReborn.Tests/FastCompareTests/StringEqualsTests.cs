using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;
using RFReborn.Pairs;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class StringEqualsTests
    {
        private readonly List<Pair<string>> _equals = new()
        {
            { "test", "test" },
            { "!%/$%@üpäüö", "!%/$%@üpäüö" },
            { @"

", @"

" },
            { null, null },
            { "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam" },
        };

        private readonly List<Pair<string>> _diff = new()
        {
            { "test", "test1" },
            { @"

", @"
" },
            { null, "" },
            { "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam", "maid des ,rtile gnicspidas rutetesnoc ,tema tis rolod muspi meroL" },
            { "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam", "Lorem ipsum dolor sit amet, consetetur,tema tis rolod muspi meroL" }
        };

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

        private static void AssertEquals(string left, string right) => Assert.IsTrue(FastCompare.Equals(left, right));

        private static void AssertDiff(string left, string right) => Assert.IsFalse(FastCompare.Equals(left, right));
    }
}
