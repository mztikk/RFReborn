using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class WildcardMatchTests
    {
        private readonly List<Pair<string>> _positiveTests = new List<Pair<string>>()
        {
            { "longteststring", "*teststring" },
            { "123456", "???456" },
            { "a", "*" },
            { "test", "*" },
            { "äöüd", "?*üd" }
        };

        private readonly List<Pair<string>> _negativeTests = new List<Pair<string>>()
        {
            { "longteststring", "*test" },
            { "123456", "???578" },
            { "plka", "" }
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
        }
    }
}
