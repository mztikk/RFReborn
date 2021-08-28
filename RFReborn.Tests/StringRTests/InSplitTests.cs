using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class InSplitTests
    {
        private readonly List<InSplitCase> _inSplits = new()
        {
            new InSplitCase("ab", 2, " ", "ab"),
            new InSplitCase("test", 2, " ", "te st"),
            new InSplitCase("aaff09bacd5a", 2, " ", "aa ff 09 ba cd 5a"),
            new InSplitCase("aaff09bacd5a", 2, "  ", "aa  ff  09  ba  cd  5a"),
            new InSplitCase("teststring", 1, "-", "t-e-s-t-s-t-r-i-n-g"),
            new InSplitCase("HelloWorld", 3, "|", "Hel|loW|orl|d"),
            new InSplitCase("HelloWorld", 0, "|", "HelloWorld"),
            new InSplitCase("test", 100, "|", "test"),
        };

        [TestMethod]
        public void InSplit()
        {
            foreach (InSplitCase split in _inSplits)
            {
                AssertSplit(split);
            }
        }

        [TestMethod]
        public void InSplitReturnStringOnNonSeperator() => Assert.AreEqual("test", StringR.InSplit("test", 2, string.Empty));

        [TestMethod]
        public void InSplitExceptionOnNullString() => Assert.ThrowsException<ArgumentNullException>(() => StringR.InSplit(null, 0, null));

        private static void AssertSplit(InSplitCase inSplitCase) => Assert.AreEqual(inSplitCase.Result, StringR.InSplit(inSplitCase.Str, inSplitCase.N, inSplitCase.Seperator));

        private class InSplitCase
        {
            public InSplitCase(string str, int n, string seperator, string result)
            {
                Str = str;
                N = n;
                Seperator = seperator;
                Result = result;
            }

            public string Str { get; }
            public int N { get; }
            public string Seperator { get; }
            public string Result { get; }
        }
    }
}
