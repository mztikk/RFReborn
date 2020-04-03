using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RFReborn.Tests
{
    public static class AssertExtensions
    {
        public static void ThrowsExceptionMessage<T>(Action action, string exceptionMessage)
        {
            Type expectedType = typeof(T);

            try
            {
                action();
            }
            catch (Exception e)
            {
                Exception val = e;
                Type actualType = val.GetType();

                Assert.AreEqual(expectedType, actualType);
                Assert.AreEqual(exceptionMessage, val.Message);

                return;
            }

            throw new AssertFailedException($"No exception thrown. Expected {expectedType.FullName}");
        }
    }
}
