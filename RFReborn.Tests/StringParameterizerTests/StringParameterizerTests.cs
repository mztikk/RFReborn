﻿using System;
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

        [TestMethod]
        public void StringParameterize()
        {
            foreach (Pair<string> item in _params)
            {
                string make = _parameterizer.Make(item.Left);
                Assert.AreEqual(item.Right, make);
            }
        }
    }
}