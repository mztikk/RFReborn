using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Disposable;

namespace RFReborn.Tests.DisposableTests
{
    [TestClass]
    public class DisposableActionTests
    {
        [TestMethod]
        public void ActionIsCalled()
        {
            bool called = false;
            void callMe() => called = true;
            using (_ = new DisposableAction(callMe)) { }

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ActionIsCalledOnlyOnce()
        {
            int calls = 0;
            void callMe() => calls++;
            var disposable = new DisposableAction(callMe);
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();

            Assert.AreEqual(1, calls);
        }
    }
}
