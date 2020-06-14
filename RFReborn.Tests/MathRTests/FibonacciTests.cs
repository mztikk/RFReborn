using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class FibonacciTests
    {
        private readonly List<KeyValuePair<int, BigInteger>> _fibs = new List<KeyValuePair<int, BigInteger>>()
        {
            new KeyValuePair<int, BigInteger>(0, 0),
            new KeyValuePair<int, BigInteger>(1, 1),
            new KeyValuePair<int, BigInteger>(2, 1),
            new KeyValuePair<int, BigInteger>(3, 2),
            new KeyValuePair<int, BigInteger>(4, 3),
            new KeyValuePair<int, BigInteger>(10, 55),
            new KeyValuePair<int, BigInteger>(17, 1597),
            new KeyValuePair<int, BigInteger>(46, 1836311903),
            new KeyValuePair<int, BigInteger>(1215, BigInteger.Parse("37196142389747318259755121959404300880074429792791530519888121160358183919766364430772194816945354836112394664539545144429405679502630059305416908854733321226667790292122976009617403666027254589674963228378604722881779796431838425618211771542168368349410")),
        };

        [TestMethod]
        public void Fibonacci()
        {
            foreach (KeyValuePair<int, BigInteger> fibonacci in _fibs)
            {
                AssertEqual(fibonacci.Key, fibonacci.Value);
            }
        }

        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-1000)]
        [DataRow(-5845)]
        [DataTestMethod]
        public void SmallerThanZeroException(int n)
        {
            void action() => MathR.Fibonacci(n);
            Assert.ThrowsException<ArgumentOutOfRangeException>(action);
            AssertExtensions.ThrowsExceptionMessage<ArgumentOutOfRangeException>(action, "N can't be smaller than zero. (Parameter 'n')");
        }

        private void AssertEqual(int n, BigInteger fibonacci) => Assert.AreEqual(fibonacci, MathR.Fibonacci(n));
    }
}
