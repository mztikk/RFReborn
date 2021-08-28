using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class FactorialTests
    {
        private readonly List<KeyValuePair<int, BigInteger>> _factorials = new()
        {
            new KeyValuePair<int, BigInteger>(0, 1),
            new KeyValuePair<int, BigInteger>(1, 1),
            new KeyValuePair<int, BigInteger>(2, 2),
            new KeyValuePair<int, BigInteger>(3, 6),
            new KeyValuePair<int, BigInteger>(4, 24),
            new KeyValuePair<int, BigInteger>(10, 3628800),
            new KeyValuePair<int, BigInteger>(17, 355687428096000),
            new KeyValuePair<int, BigInteger>(46, BigInteger.Parse("5502622159812088949850305428800254892961651752960000000000")),
        };

        [TestMethod]
        public void Factorial()
        {
            foreach (KeyValuePair<int, BigInteger> factorial in _factorials)
            {
                AssertEqual(factorial.Key, factorial.Value);
            }
        }

        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-1000)]
        [DataRow(-5845)]
        [DataTestMethod]
        public void SmallerThanZeroException(int n)
        {
            void action() => MathR.Factorial(n);
            Assert.ThrowsException<ArgumentOutOfRangeException>(action);
            AssertExtensions.ThrowsExceptionMessage<ArgumentOutOfRangeException>(action, "N can't be smaller than zero. (Parameter 'n')");
        }

        private static void AssertEqual(int n, BigInteger factorial) => Assert.AreEqual(factorial, MathR.Factorial(n));
    }
}
