using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
using RFReborn.Pairs;

namespace RFReborn.Tests.OpsTests
{
    [TestClass]
    public class SwapTests
    {
        private readonly List<Pair<int>> _pairs = new List<Pair<int>>() { { 1, 2 }, { 5, 10 }, { 100, 200 } };

        [TestMethod]
        public void Swap()
        {
            foreach (Pair<int> pair in _pairs)
            {
                AssertSwapped(pair.Left, pair.Right);
            }
        }

        private void AssertSwapped<T>(T m1, T m2)
        {
            T m1copy = m1;
            T m2copy = m2;
            Assert.AreEqual(m1, m1copy);
            Assert.AreEqual(m2, m2copy);

            Ops.Swap(ref m1, ref m2);

            Assert.AreEqual(m1copy, m2);
            Assert.AreEqual(m2copy, m1);

            Assert.AreNotEqual(m1, m1copy);
            Assert.AreNotEqual(m2, m2copy);
        }
    }
}
