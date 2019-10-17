using System.IO;
using RFReborn.AoB;

namespace RFReborn.Tests.AoBTests
{
    internal class AoBTest
    {
        public byte[] SearchRegion { get; set; }

        public Signature Signature { get; set; }

        public AoBTest() { }

        public AoBTest(byte[] searchRegion, Signature signature)
        {
            SearchRegion = searchRegion;
            Signature = signature;
        }

        public Stream GetSearchRegionAsStream() => new MemoryStream(SearchRegion);
    }
}
