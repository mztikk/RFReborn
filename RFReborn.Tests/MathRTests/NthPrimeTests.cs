using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class NthPrimeTests
    {
        private readonly List<KeyValuePair<int, long>> _primes = new List<KeyValuePair<int, long>>()
        {
            new KeyValuePair<int, long>(1, 2),
            new KeyValuePair<int, long>(2, 3),
            new KeyValuePair<int, long>(3, 5),
            new KeyValuePair<int, long>(4, 7),
            new KeyValuePair<int, long>(10, 29),
            new KeyValuePair<int, long>(17, 59),
            new KeyValuePair<int, long>(46, 199),
            new KeyValuePair<int, long>(981, 7727)
        };

        [TestMethod]
        public void NthPrime()
        {
            foreach (KeyValuePair<int, long> prime in _primes)
            {
                AssertEquals(prime.Key, prime.Value);
            }
        }

        private void AssertEquals(int n, long prime)
        {
            Assert.AreEqual(prime, MathR.NthPrime(n));
        }
    }
}
