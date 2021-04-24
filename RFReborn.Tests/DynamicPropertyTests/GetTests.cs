using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Internals;

namespace RFReborn.Tests.DynamicPropertyTests
{
    [TestClass]
    public class GetTests
    {
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(50)]
        public void GetIntDirect(int n)
        {
            var test = new { N = n };
            Assert.AreEqual(n, DynamicProperty.Get(test, "N"));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(50)]
        public void GetChainedInt(int n)
        {
            var test = new { inner = new { N = n } };
            Assert.AreEqual(n, DynamicProperty.Get(test.GetType(), test, new[] { "inner", "N" }));
        }
    }
}
