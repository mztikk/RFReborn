using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class RemoveWhitespaceTests
    {
        private readonly List<Pair<string>> _tests = new List<Pair<string>>() { { "abc abc", "abcabc" }, { " üö üö", "üöüö" }, { " @$% &/(%345 ", "@$%&/(%345" } };

        [TestMethod]
        public void RemoveWhitespace()
        {
            foreach (Pair<string> test in _tests)
            {
                RemoveWhitespaceAssert(test.Right, test.Left);
                AssertNoSpace(test.Left);
            }
        }

        private void RemoveWhitespaceAssert(string expected, string toTest) => Assert.AreEqual(expected, StringR.RemoveWhitespace(toTest));

        private void AssertNoSpace(string toTest) => Assert.IsFalse(StringR.RemoveWhitespace(toTest).Contains(" "));
    }
}
