using System;
using BenchmarkDotNet.Attributes;
using RFReborn.AoB;

namespace RFReborn.Benchmark.Benchmarks
{
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [MarkdownExporterAttribute.Default]
    [MarkdownExporterAttribute.GitHub]
    public class AoBScannerBenchmark
    {
        [Params(64, 1024)]
        public int SearchRegionSize;

        public byte[] SearchRegion;

        [Params(8, 16)]
        public int SignatureLength;

        public Signature SignatureStart;
        public Signature SignatureMiddle;
        public Signature SignatureEnd;

        private readonly Random _random = new Random();

        [GlobalSetup]
        public void GlobalSetup()
        {
            SearchRegion = new byte[SearchRegionSize];
            _random.NextBytes(SearchRegion);

            SignatureStart = BuildSignature(1);
            SignatureMiddle = BuildSignature(SearchRegionSize / 2);
            SignatureEnd = BuildSignature(SearchRegionSize - SignatureLength - 2);
        }

        private Signature BuildSignature(int startIndex)
        {
            byte[] pattern = new byte[SignatureLength];
            string mask = "";
            for (int i = 0; i < pattern.Length; i++)
            {
                byte b = SearchRegion[startIndex + i];
                if (i % 4 == 0)
                {
                    mask += "?";
                }
                else
                {
                    mask += "x";
                }

                pattern[i] = b;
            }

            return new Signature(pattern, mask);
        }

        [Benchmark]
        public long FindSignatureStart() => Scanner.FindSignature(SearchRegion, SignatureStart);
        [Benchmark]
        public long FindSignatureMiddle() => Scanner.FindSignature(SearchRegion, SignatureMiddle);
        [Benchmark]
        public long FindSignatureEnd() => Scanner.FindSignature(SearchRegion, SignatureEnd);
    }
}
