using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private void AssertRemoved(string toTest, char[] toRemove) => Assert.IsFalse(Contains(StringR.RemoveChars(toTest, toRemove), toRemove));

        private static bool Contains(string str, char[] chrs) => str.IndexOfAny(chrs) > 0;
    }
}
