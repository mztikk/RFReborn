using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.Comparison;

namespace RFReborn.Tests.FastCompareTests
{
    [TestClass]
    public class StreamEqualsTests
    {
        private const int Seed = 123456789;

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

        [TestMethod]
        public async Task StreamEqualsAsync()
        {
            byte[] zerobytes = new byte[9001];
            Array.Fill<byte>(zerobytes, 0);

            byte[] onebytes = new byte[9001];
            Array.Fill<byte>(onebytes, 1);

            await AssertEqualAsync(zerobytes, zerobytes).ConfigureAwait(false);
            await AssertEqualAsync(onebytes, onebytes).ConfigureAwait(false);

            byte[] loremipsumbytes = Encoding.UTF8.GetBytes(LoremIpsum);
            byte[] loremipsumreversebytes = Encoding.UTF8.GetBytes(LoremIpsumReverse);

            await AssertEqualAsync(loremipsumbytes, loremipsumbytes).ConfigureAwait(false);
            await AssertEqualAsync(loremipsumreversebytes, loremipsumreversebytes).ConfigureAwait(false);
        }

        [TestMethod]
        public void SmallEqualArrays()
        {
            Random r = new Random(Seed);
            byte[] left = new byte[15];
            byte[] right = new byte[10];

            r.NextBytes(left);
            Array.Copy(left, right, right.Length);

            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsTrue(FastCompare.Equals(leftstream, rightstream, right.Length));
                }
            }
        }

        [TestMethod]
        public void StreamDifferent()
        {
            Random r = new Random(Seed);
            byte[] left = new byte[8192 * 4];
            byte[] right = new byte[8192 * 4];

            r.NextBytes(left);
            r.NextBytes(right);

            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream, left.Length));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsTrue(FastCompare.NotEquals(leftstream, rightstream));
                }
            }
        }

        [TestMethod]
        public void DifferentLengthFull()
        {
            Random r = new Random(Seed);
            byte[] left = new byte[8192 * 4];
            byte[] right = new byte[8192 * 3];

            r.NextBytes(left);
            Array.Copy(left, right, right.Length);

            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsTrue(FastCompare.NotEquals(leftstream, rightstream));
                }
            }
        }

        [TestMethod]
        public void DifferentPositionFull()
        {
            Random r = new Random(Seed);
            byte[] left = new byte[8192 * 4];
            byte[] right = new byte[8192 * 4];

            r.NextBytes(left);
            Array.Copy(left, right, right.Length);

            using (MemoryStream leftstream = new MemoryStream(left))
            {
                leftstream.Seek(10, SeekOrigin.Begin);
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream));
                    leftstream.Seek(10, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsTrue(FastCompare.NotEquals(leftstream, rightstream));
                }
            }
        }

        [TestMethod]
        public void DifferentLength()
        {
            Random r = new Random(Seed);
            byte[] left = new byte[8192 * 4];
            byte[] right = new byte[8192 * 3];

            r.NextBytes(left);
            Array.Copy(left, right, right.Length);

            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    Assert.IsTrue(FastCompare.Equals(leftstream, rightstream, right.Length));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsFalse(FastCompare.Equals(leftstream, rightstream, left.Length));
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsTrue(FastCompare.NotEquals(leftstream, rightstream));
                }
            }
        }

        private void AssertEqual(byte[] left, byte[] right)
        {
            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    AssertEqual(leftstream, rightstream);
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    AssertEqual(leftstream, rightstream, left.Length);
                    leftstream.Seek(0, SeekOrigin.Begin);
                    rightstream.Seek(0, SeekOrigin.Begin);
                    Assert.IsFalse(FastCompare.NotEquals(leftstream, rightstream));
                }
            }
        }

        private async Task AssertEqualAsync(byte[] left, byte[] right)
        {
            using (MemoryStream leftstream = new MemoryStream(left))
            {
                using (MemoryStream rightstream = new MemoryStream(right))
                {
                    await AssertEqualAsync(leftstream, rightstream).ConfigureAwait(false);
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
        private static async Task AssertEqualAsync(Stream left, Stream right) => Assert.IsTrue(await FastCompare.EqualsAsync(left, right).ConfigureAwait(false));
        private static void AssertDiff(Stream left, Stream right) => Assert.IsTrue(FastCompare.NotEquals(left, right));
        private static void AssertEqual(Stream left, Stream right, int len) => Assert.IsTrue(FastCompare.Equals(left, right, len));
    }
}

