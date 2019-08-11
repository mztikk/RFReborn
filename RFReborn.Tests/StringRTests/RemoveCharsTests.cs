using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class RemoveCharsTests
    {
        private readonly List<Pair<string>> _tests = new List<Pair<string>>() { { "test", "es" } };

        [TestMethod]
        public void RemoveChars()
        {
            foreach (Pair<string> test in _tests)
            {
                AssertRemoved(test.Left, test.Right.ToCharArray());
            }
        }

        private void AssertRemoved(string toTest, char[] toRemove)
        {
            Assert.IsFalse(contains(StringR.RemoveChars(toTest, toRemove), toRemove));
        }

        private bool contains(string str, char[] chrs) => str.IndexOfAny(chrs) > 0;
    }
}
