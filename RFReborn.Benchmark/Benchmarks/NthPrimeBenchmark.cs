using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace RFReborn.Benchmark.Benchmarks
{
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [MarkdownExporterAttribute.Default]
    [MarkdownExporterAttribute.GitHub]
    public class NthPrimeBenchmark
    {
        [Params(1, 17, 64, 1024)]
        public int N;

        [Benchmark]
        public BigInteger NthPrime() => MathR.NthPrime(N);

    }
}
