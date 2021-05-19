using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.RandomExtensionsTests
{
    [TestClass]
    public class NextDoubleTests
    {
        private const int Iterations = 1000;

        [DataTestMethod]
        [DataRow(1d, 2d)]
        [DataRow(1d, 1.5d)]
        public void IsBetweenMinAndMaxValue(double minValue, double maxValue)
        {
            for (int i = 0; i < Iterations; i++)
            {
                Random rnd = new Random();
                double val = rnd.NextDouble(minValue, maxValue);
                Assert.IsTrue(
                    val >= minValue && val < maxValue,
                    $"Value({val}) should be greater or equal to MinValue({minValue}) and less than MaxValue({maxValue})");
            }
        }

        [DataTestMethod]
        [DataRow(1d, 1d)]
        [DataRow(1.5d, 1.5d)]
        public void EqualMinAndMaxIsMin(double minValue, double maxValue)
        {
            for (int i = 0; i < Iterations; i++)
            {
                Random rnd = new Random();
                double val = rnd.NextDouble(minValue, maxValue);
                Assert.IsTrue(
                    val == minValue,
                    $"Value({val}) should be equal to MinValue({minValue}))");
            }
        }

        [DataTestMethod]
        [DataRow(1d)]
        [DataRow(1.5d)]
        public void IsLessThanMax(double maxValue)
        {
            for (int i = 0; i < Iterations; i++)
            {
                Random rnd = new Random();
                double val = rnd.NextDouble(maxValue);
                Assert.IsTrue(
                    val < maxValue,
                    $"Value({val}) should be less than MaxValue({maxValue}))");
            }
        }

        [DataTestMethod]
        [DataRow(1d)]
        [DataRow(1.5d)]
        [DataRow(0d)]
        public void IsPositive(double maxValue)
        {
            for (int i = 0; i < Iterations; i++)
            {
                Random rnd = new Random();
                double val = rnd.NextDouble(maxValue);
                Assert.IsFalse(
                    val < 0,
                    $"Value({val}) should be non-negative)");
            }
        }

        [DataTestMethod]
        [DataRow(-1d)]
        [DataRow(-1.5d)]
        public void NegativeThrows(double maxValue)
        {
            Random rnd = new Random();
            Assert.ThrowsException<ArgumentException>(() => rnd.NextDouble(maxValue));
        }
    }
}
