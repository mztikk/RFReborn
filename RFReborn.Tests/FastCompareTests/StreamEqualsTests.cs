using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class StreamEqualsTests
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
        private const string LoremIpsumReverse = ".tema tis rolod muspi meroL tse sutcnas atamikat aes on ,nergrebug dsak atilc tetS .muber ae te serolod oud otsuj te masucca te soe orev tA .autpulov maid des ,tare mayuqila angam erolod te erobal tu tnudivni ropmet domrie ymunon maid des ,rtile gnicspidas rutetesnoc ,tema tis rolod muspi meroL";

        [TestMethod]
        public void StreamEquals()
        {
            byte[] zerobytes = new byte[9001];
            Array.Fill<byte>(zerobytes, 0);

            byte[] onebytes = new byte[9001];
            Array.Fill<byte>(onebytes, 1);

            AssertEqual(zerobytes, zerobytes);
            AssertEqual(onebytes, onebytes);
            AssertDiff(zerobytes, onebytes);

            byte[] loremipsumbytes = Encoding.UTF8.GetBytes(LoremIpsum);
            byte[] loremipsumreversebytes = Encoding.UTF8.GetBytes(LoremIpsumReverse);

            AssertEqual(loremipsumbytes, loremipsumbytes);
            AssertEqual(loremipsumreversebytes, loremipsumreversebytes);
            AssertDiff(loremipsumbytes, loremipsumreversebytes);
        }

        private void AssertEqual(byte[] left, byte[] right)
        {
            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    AssertEqual(leftstream, rightstream);
                }
            }
        }

        private void AssertDiff(byte[] left, byte[] right)
        {
            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    AssertDiff(leftstream, rightstream);
                }
            }
        }

        private static void AssertEqual(Stream left, Stream right) => Assert.IsTrue(FastCompare.Equals(left, right));
        private static void AssertDiff(Stream left, Stream right) => Assert.IsTrue(FastCompare.NotEquals(left, right));
    }
}

