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
        private readonly StringParameterizer _parameterizer = new(new List<KeyValuePair<string, Func<string>>>()
        {
            { "World", () => "World!" },
            { "Num", () => "1337" },
        });

        private readonly List<Pair<string>> _params = new()
        {
            { "Hello{World}", "HelloWorld!" },
            { "{World}={Num}", "World!=1337" },
            { "<{Timestamp HH:mm}>", $"<{DateTime.Now:HH:mm}>" },
        };

        private readonly StringParameterizer _parameterizer2 = new();

        private readonly List<Pair<string>> _params2 = new()
        {
            { "Hello<-!World!->", "HelloWorld!" },
            { "<-!World!->=<-!Num!->", "World!=1337" },
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

        [DataTestMethod]
        [DataRow("HH")]
        [DataRow("hh")]
        [DataRow("hhmm")]
        [DataRow("HHmm")]
        [DataRow("HH:mm")]
        [DataRow("yyyyMMdd")]
        public void TimestampHours(string timestamp) => Assert.AreEqual(DateTime.Now.ToString(timestamp), _parameterizer.Make($"{{{_parameterizer.Timestamp} {timestamp}}}"));

        //[TestMethod]
        //public void StreamParameterize()
        //{
        //    foreach (Pair<string> item in _params)
        //    {
        //        using (MemoryStream target = new MemoryStream())
        //        {
        //            _parameterizer.Make(item.Left.GetStream(), target, (string _) => null);
        //            target.Position = 0;
        //            Assert.AreEqual(item.Right, new StreamReader(target).ReadToEnd()); ;
        //        }
        //    }

        //    foreach (Pair<string> item in _params2)
        //    {
        //        using (MemoryStream target = new MemoryStream())
        //        {
        //            _parameterizer.Make(item.Left.GetStream(), target, (string _) => null);
        //            target.Position = 0;
        //            Assert.AreEqual(item.Right, new StreamReader(target).ReadToEnd());
        //        }
        //    }
        //}
    }
}
