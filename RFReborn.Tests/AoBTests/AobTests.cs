﻿using System.Collections.Generic;
using System.IO;
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
            { AoBTestGenerator.ForSignature(new Signature("ff??aeff"), BufferSize * 2, BufferSize - 3) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 23) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 22) },
            { AoBTestGenerator.ForSignature(new Signature("48 ?? C2 48 8D 0D B9 DC 63 02 FF C2 ?? 15 71 E6 63 ?? 8B 0C 81 8B C1"), BufferSize * 2, BufferSize - 21) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 1) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 2) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize - 3) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 1) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 2) },
            { AoBTestGenerator.ForSignature(new Signature("48 63 C2 48 8D 0D B9 DC 63 02 FF C2 89 15 71 E6 63 02 8B 0C 81 8B C1 C1 E8 0B 33 C8 8B C1 25 AD 58 3A FF C1 E0 07 33 C8 8B C1 25 8C DF FF FF C1 E0 0F 33 C8 8B C1 C1 E8 12 33 C1 48 83 C4 28"), BufferSize * 2, BufferSize + 3) },
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
    }
}
