using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Extensions;

namespace RFReborn.Tests.FuncExtensionsTests
{
    [TestClass]
    public class TryTilTrueTests
    {
        [TestMethod]
        public void TryTilTrueMaxTries()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnFalse = () =>
            {
                i++;
                return false;
            };
            returnFalse.TryTilTrue(max, 1);
            Assert.AreEqual(max, i);
        }

        [TestMethod]
        public void TryTilTrueTrue()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnTrue = () =>
            {
                i++;
                return true;
            };
            returnTrue.TryTilTrue(max, 1);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void TryTilTrueHalfway()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnTrue = () =>
            {
                i++;
                return i > max / 2;
            };
            returnTrue.TryTilTrue(max, 1);
            Assert.AreEqual((max / 2) + 1, i);
        }

        [TestMethod]
        public async Task TryTilTrueTaskAsyncMaxTries()
        {
            const int max = 10;
            int i = 0;
            Func<Task<bool>> returnFalse = () =>
            {
                i++;
                return Task.FromResult(false);
            };
            await returnFalse.TryTilTrue(max, 1);
            Assert.AreEqual(max, i);
        }

        [TestMethod]
        public async Task TryTilTrueTaskAsyncTrue()
        {
            const int max = 10;
            int i = 0;
            Func<Task<bool>> returnTrue = () =>
            {
                i++;
                return Task.FromResult(true);
            };
            await returnTrue.TryTilTrue(max, 1);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task TryTilTrueTaskAsyncHalfway()
        {
            const int max = 10;
            int i = 0;
            Func<Task<bool>> returnTrue = () =>
            {
                i++;
                return Task.FromResult(i > max / 2);
            };
            await returnTrue.TryTilTrue(max, 1);
            Assert.AreEqual((max / 2) + 1, i);
        }

        [TestMethod]
        public async Task TryTilTrueAsyncMaxTries()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnFalse = () =>
            {
                i++;
                return false;
            };
            await returnFalse.TryTilTrueAsync(max, 1);
            Assert.AreEqual(max, i);
        }

        [TestMethod]
        public async Task TryTilTrueAsyncTrue()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnTrue = () =>
            {
                i++;
                return true;
            };
            await returnTrue.TryTilTrueAsync(max, 1);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task TryTilTrueAsyncHalfway()
        {
            const int max = 10;
            int i = 0;
            Func<bool> returnTrue = () =>
            {
                i++;
                return i > max / 2;
            };
            await returnTrue.TryTilTrueAsync(max, 1);
            Assert.AreEqual((max / 2) + 1, i);
        }
    }
}
