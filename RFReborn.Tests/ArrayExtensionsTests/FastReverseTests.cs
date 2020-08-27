using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.ArrayExtensionsTests
{
    [TestClass]
    public class FastReverseTests
    {
        [TestMethod]
        public void FastReverse()
        {
            int[] testArray = new[] { 1, 2, 3, 4, 5 };
            int[] testArrayReverse = new[] { 5, 4, 3, 2, 1 };

            CollectionAssert.AreNotEqual(testArray, testArrayReverse);
            testArray.FastReverse();
            CollectionAssert.AreEqual(testArray, testArrayReverse);
        }
    }
}
