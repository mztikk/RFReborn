using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.ArrayExtensionsTests
{
    [TestClass]
    public class IndicesOfTests
    {
        [TestMethod]
        public void IndicesOf()
        {
            int[] testList = new[] { 0, 1, 1, 0, 1, 0, 1 };
            int[] zeroIndices = new[] { 0, 3, 5 };
            int[] oneIndices = new[] { 1, 2, 4, 6 };
            IEnumerable<int> testZeroIndices = testList.IndicesOf(0);
            IEnumerable<int> testOneIndices = testList.IndicesOf(1);

            Assert.IsTrue(testZeroIndices.SequenceEqual(zeroIndices));
            Assert.IsTrue(testOneIndices.SequenceEqual(oneIndices));
        }
    }
}
