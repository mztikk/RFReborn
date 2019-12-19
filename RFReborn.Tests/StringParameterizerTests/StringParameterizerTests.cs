using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;
using RFReborn.Pairs;

namespace RFReborn.Tests.StringParameterizerTests
{
    [TestClass]
    public class StringParameterizerTests
    {
        private readonly StringParameterizer _parameterizer = new StringParameterizer(new List<KeyValuePair<string, Func<string>>>()
        {
            { "World", () => "World!" },
            { "Num", () => "1337" },
        });

        private readonly List<Pair<string>> _params = new List<Pair<string>>()
        {
            {"Hello{World}", "HelloWorld!" },
            {"{World}={Num}", "World!=1337" },
        };

        private readonly StringParameterizer _parameterizer2 = new StringParameterizer();

        private readonly List<Pair<string>> _params2 = new List<Pair<string>>()
        {
            {"Hello<-!World!->", "HelloWorld!" },
            {"<-!World!->=<-!Num!->", "World!=1337" },
        };

        [TestInitialize]
        public void Init()
        {
            _parameterizer2.OpenTag = "<-!";
            _parameterizer2.CloseTag = "!->";

            _parameterizer2.Add("World", () => "World!");
            _parameterizer2.Add("Num", () => "1337");
        }

        [TestMethod]
        public void StringParameterize()
        {
            foreach (Pair<string> item in _params)
            {
                string make = _parameterizer.Make(item.Left);
                Assert.AreEqual(item.Right, make);
            }

            foreach (Pair<string> item in _params2)
            {
                string make = _parameterizer2.Make(item.Left);
                Assert.AreEqual(item.Right, make);
            }
        }

        [TestMethod]
        public void StreamParameterize()
        {
            foreach (Pair<string> item in _params)
            {
                string streamMake = _parameterizer.Make(item.Left.GetStream(), (string _) => null);
                Assert.AreEqual(item.Right, streamMake);
            }

            foreach (Pair<string> item in _params2)
            {
                string streamMake = _parameterizer2.Make(item.Left.GetStream(), (string _) => null);
                Assert.AreEqual(item.Right, streamMake);
            }
        }
    }
}
