using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class SplitNTests
    {
        private readonly List<SplitNCase> _tests = new List<SplitNCase>()
        {
            new SplitNCase("test", 1, new string[]{ "t", "e", "s", "t" }),
            new SplitNCase("test", 2, new string[]{ "te", "st" }),
            new SplitNCase("test", 3, new string[]{ "tes", "t" }),
            new SplitNCase("test", 4, new string[]{ "test" }),
        };

        [TestMethod]
        public void SplitN()
        {
            foreach (SplitNCase test in _tests)
            {
                string[] split = StringR.SplitN(test.S, test.N);
                Assert.AreEqual(test.Expected.Length, split.Length);

                for (int i = 0; i < split.Length; i++)
                {
                    Assert.AreEqual(test.Expected[i], split[i]);
                }
            }
        }

        private class SplitNCase
        {
            public SplitNCase(string s, int n, string[] expected)
            {
                S = s;
                N = n;
                Expected = expected;
            }

            public string S { get; }
            public int N { get; }
            public string[] Expected { get; }
        }
    }
}
