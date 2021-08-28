using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringRTests
{
    [TestClass]
    public class CapitalizeFirstLetterTests
    {
        private readonly List<Pair<string>> _capitalizeTests = new() { { "hello World!", "Hello World!" }, { "öäü", "Öäü" } };

        private readonly List<Pair<string>> _capitalizeNoChangeTests = new() { { "!hello World!", "!hello World!" }, { "ßss", "ßss" }, { "???", "???" }, { "{{}}", "{{}}" } };

        [TestMethod]
        public void CapitalizeFirstLetter()
        {
            foreach (Pair<string> test in _capitalizeTests)
            {
                AssertCapitalized(test.Left, test.Right);
            }
        }

        [TestMethod]
        public void CapitalizeFirstLetterNoChange()
        {
            foreach (Pair<string> test in _capitalizeNoChangeTests)
            {
                AssertCapitalizedNoChange(test.Left, test.Right);
            }
        }

        [TestMethod]
        public void CapitalizeSelfFirstLetter()
        {
            foreach (Pair<string> test in _capitalizeTests)
            {
                string og = new(test.Left);
                AssertCapitalizedSelf(og, test.Right);
            }
        }

        [TestMethod]
        public void CapitalizeSelfFirstLetterNoChange()
        {
            foreach (Pair<string> test in _capitalizeNoChangeTests)
            {
                string og = new(test.Left);
                AssertCapitalizedNoChangeSelf(og, test.Right);
            }
        }

        private static void AssertCapitalized(string original, string expected)
        {
            Assert.AreNotEqual(expected, original);
            string capitalized = original.CapitalizeFirstLetter();
            Assert.AreEqual(expected, capitalized);
            Assert.AreNotEqual(expected, original);
            Assert.AreNotEqual(original, capitalized);
        }

        private static void AssertCapitalizedSelf(string original, string expected)
        {
            Assert.AreNotEqual(expected, original);
            original.CapitalizeFirstLetterSelf();
            Assert.AreEqual(expected, original);
        }

        private static void AssertCapitalizedNoChange(string original, string expected)
        {
            Assert.AreEqual(expected, original);
            string capitalized = original.CapitalizeFirstLetter();
            Assert.AreEqual(expected, capitalized);
        }

        private static void AssertCapitalizedNoChangeSelf(string original, string expected)
        {
            Assert.AreEqual(expected, original);
            original.CapitalizeFirstLetterSelf();
            Assert.AreEqual(expected, original);
        }
    }
}
