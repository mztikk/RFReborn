using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;

namespace RFReborn.Tests.OpsTests
{
    [TestClass]
    public class MergeTests
    {
        [TestMethod]
        public void Merge()
        {
            Assert.IsTrue(FastCompare.Equals(new[] { 1, 2, 3, 4 }, Ops.Merge(new[] { 1 }, new[] { 2, 3, 4, }).ToArray()));
            Assert.IsTrue(FastCompare.Equals(new[] { 1, 2, 3, 4 }, Ops.Merge(new[] { 1, 2 }, new[] { 3, 4, }).ToArray()));
            Assert.IsTrue(FastCompare.Equals(new[] { 1, 2, 3, 4 }, Ops.Merge(new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 4 }).ToArray()));
        }
    }
}
