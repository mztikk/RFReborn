using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests.MathRTests
{
    [TestClass]
    public class RotateLeftTests
    {
        [DataRow(123u, 1, 246u)]
        [DataRow(246u, 5, 7872u)]
        [DataRow(1337u, 8, 342272u)]
        [DataTestMethod]
        public void RotateLeftUInt(uint input, int n, uint result) => Assert.AreEqual(result, MathR.RotateLeft(input, n));

        [DataRow(123ul, 1, 246ul)]
        [DataRow(246ul, 5, 7872ul)]
        [DataRow(1337ul, 8, 342272ul)]
        [DataTestMethod]
        public void RotateLeftULong(ulong input, int n, ulong result) => Assert.AreEqual(result, MathR.RotateLeft(input, n));
    }
}
