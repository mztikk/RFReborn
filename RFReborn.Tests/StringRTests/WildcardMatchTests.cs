using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class WildcardMatchTests
    {
        private readonly List<Pair<string>> _positiveTests = new()
        {
            { "longteststring", "*teststring" },
            { "123456", "???456" },
            { "a", "*" },
            { "test", "*" },
            { "äöüd", "?*üd" },
            { "abc", "abc" },
            { "longteststring", "long*" },
            { "abcd", "abc?" },
            { "abcd", "***d" }
        };

        private readonly List<Pair<string>> _positiveCasingTests = new()
        {
            { "lonGteStstring", "*teststring" },
            { "äöÜd", "?*üd" },
            { "abc", "ABC" },
            { "longteststring", "LONG*" },
            { "aBcD", "abc?" },
            { "abCD", "***d" }
        };

        private readonly List<Pair<string>> _negativeTests = new()
        {
            { "longteststring", "*test" },
            { "123456", "???578" },
            { "plka", "" },
            { "abcd", "abc" },
            { "abc", "abcd" },
            { "longteststring", "long?" },
            { "abc", "**d" },
        };

        [TestMethod]
        public void WildcardMatch()
        {
            foreach (Pair<string> positiveTest in _positiveTests)
            {
                Assert.IsTrue(StringR.WildcardMatch(positiveTest.Left, positiveTest.Right));
            }

            foreach (Pair<string> negativeTest in _negativeTests)
            {
                Assert.IsFalse(StringR.WildcardMatch(negativeTest.Left, negativeTest.Right));
            }

            foreach (Pair<string> positiveCasingTest in _positiveCasingTests)
            {
                Assert.IsTrue(StringR.WildcardMatch(positiveCasingTest.Left, positiveCasingTest.Right, true));
            }

            foreach (Pair<string> positiveCasingTest in _positiveCasingTests)
            {
                Assert.IsFalse(StringR.WildcardMatch(positiveCasingTest.Left, positiveCasingTest.Right, false));
            }
        }
    }
}
