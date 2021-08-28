using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class IsPrimeTests
    {
        [DataTestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(11)]
        [DataRow(13)]
        [DataRow(17)]
        [DataRow(19)]
        [DataRow(23)]
        [DataRow(6701)]
        [DataRow(492876863)]
        [DataRow(982451653)]
        public void IsPrime(int prime) => AssertIsPrime(prime);

        [DataTestMethod]
        [DataRow(-123)]
        [DataRow(-5)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(6)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(12)]
        [DataRow(14)]
        [DataRow(15)]
        [DataRow(16)]
        [DataRow(18)]
        [DataRow(20)]
        [DataRow(21)]
        [DataRow(22)]
        [DataRow(7828)]
        [DataRow(492876860)]
        public void IsNotPrime(int prime) => AssertIsNotPrime(prime);

        private static void AssertIsPrime(int n) => Assert.IsTrue(MathR.IsPrime(n));

        private static void AssertIsNotPrime(int n) => Assert.IsFalse(MathR.IsPrime(n));
    }
}
