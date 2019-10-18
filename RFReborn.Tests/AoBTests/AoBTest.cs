using System.IO;
using RFReborn.AoB;

namespace RFReborn.Tests.AoBTests
{
    internal class AoBTest
    {
        public byte[] SearchRegion { get; set; }

        public Signature Signature { get; set; }

        public long Index { get; set; }

        public AoBTest() { }

        public AoBTest(byte[] searchRegion, Signature signature, long index)
        {
            SearchRegion = searchRegion;
            Signature = signature;
            Index = index;
        }

        public Stream GetSearchRegionAsStream() => new MemoryStream(SearchRegion);
    }
}
