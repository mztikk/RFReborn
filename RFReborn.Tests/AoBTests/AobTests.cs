using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFReborn.AoB;

namespace RFReborn.Tests.AoBTests
{
    [TestClass]
    public class AobTests
    {
        private const int BufferSize = 4048;

        private readonly List<AoBTest> _tests = new List<AoBTest>()
        {
            { AoBTestGenerator.ForSignature(new Signature("05010203")) },
            { AoBTestGenerator.ForSignature(new Signature("ff??aeff")) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D"), 5, 0) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D"), 6, 0) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D"), 6, 1) },
            { AoBTestGenerator.ForSignature(new Signature("ff??aeff"), BufferSize * 2, BufferSize - 3) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 23) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 22) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 21) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize, BufferSize -63) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize + 3, BufferSize -60) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize + 21, BufferSize -42) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 1) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 2) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 3) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 1) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 2) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 3) },
        };

        private readonly List<AoBTest> _fails = new List<AoBTest>()
        {
            {new AoBTest(new byte[]{0xFF }, new Signature("FFEE"), -1) },
            {new AoBTest(new byte[]{0xFF, 0x00, 0xAE }, new Signature("FFEE"), -1) },
            {new AoBTest(new byte[]{0xFF, 0x00, 0xAE, 0xFF }, new Signature("FFEE"), -1) },
        };

        [TestMethod]
        public void SignatureInByteArray()
        {
            foreach (AoBTest test in _tests)
            {
                AssertFound(test.SearchRegion, test.Signature, test.Index);
            }
        }

        [TestMethod]
        public void SignatureInStream()
        {
            foreach (AoBTest test in _tests)
            {
                AssertFound(test.GetSearchRegionAsStream(), test.Signature, test.Index);
            }
        }

        [TestMethod]
        public void SignatureNotInByteArray()
        {
            foreach (AoBTest test in _fails)
            {
                AssertNotFound(test.SearchRegion, test.Signature);
            }
        }

        [TestMethod]
        public void SignatureNotInStream()
        {
            foreach (AoBTest test in _fails)
            {
                AssertNotFound(test.GetSearchRegionAsStream(), test.Signature);
            }
        }

        [TestMethod]
        public void MultipleSignatures()
        {
            byte[] bytes = new byte[] { 0xff, 0xe1, 0xcd, 0xff, 0xe1, 0xcd, 0xff, 0xff, 0xe1, 0xcd };
            Signature sig = new Signature("E1 CD");
            long[] indices = new long[] { 1, 4, 8 };
            long[] foundIndices = Scanner.FindSignatures(bytes, sig).ToArray();
            Assert.AreEqual(indices.Length, foundIndices.Length);
            for (int i = 0; i < foundIndices.Length; i++)
            {
                Assert.AreEqual(indices[i], foundIndices[i]);
            }
        }

        private void AssertFound(byte[] searchRegion, Signature signature, long toFind)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreEqual(toFind, find);
        }

        private void AssertFound(Stream searchRegion, Signature signature, long toFind)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreEqual(toFind, find);
        }

        private void AssertNotFound(byte[] searchRegion, Signature signature)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreEqual(-1, find);
        }

        private void AssertNotFound(Stream searchRegion, Signature signature)
        {
            long find = Scanner.FindSignature(searchRegion, signature);
            Assert.AreEqual(-1, find);
        }
    }
}
