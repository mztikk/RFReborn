using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class NthPrimeTests
    {
        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 3)]
        [DataRow(3, 5)]
        [DataRow(4, 7)]
        [DataRow(10, 29)]
        [DataRow(17, 59)]
        [DataRow(46, 199)]
        [DataRow(981, 7727)]
        public void NthPrime(int n, long prime) => AssertEquals(n, prime);

        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-1000)]
        [DataRow(-5845)]
        [DataTestMethod]
        public void SmallerThanOneException(int n)
        {
            void action() => MathR.NthPrime(n);
            Assert.ThrowsException<ArgumentOutOfRangeException>(action);
            AssertExtensions.ThrowsExceptionMessage<ArgumentOutOfRangeException>(action, "N can't be smaller than one. (Parameter 'n')");
        }

        private static void AssertEquals(int n, long prime) => Assert.AreEqual(prime, MathR.NthPrime(n));
    }
}
