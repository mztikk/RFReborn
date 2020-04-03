using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class IsPrimeTests
    {
        private readonly List<int> _primes = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 6701, 492876863, 982451653 };

        private readonly List<int> _nonPrimes = new List<int>() { -123, -5, 0, 1, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 7828, 492876860 };

        [TestMethod]
        public void IsPrime()
        {
            foreach (int prime in _primes)
            {
                AssertIsPrime(prime);
            }

            foreach (int nonPrime in _nonPrimes)
            {
                AssertIsNotPrime(nonPrime);
            }
        }

        private void AssertIsPrime(int n) => Assert.IsTrue(MathR.IsPrime(n));

        private void AssertIsNotPrime(int n) => Assert.IsFalse(MathR.IsPrime(n));
    }
}
