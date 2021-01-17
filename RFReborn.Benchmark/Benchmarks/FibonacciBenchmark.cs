using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace RFReborn.Benchmark.Benchmarks
{
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [MarkdownExporterAttribute.Default]
    [MarkdownExporterAttribute.GitHub]
    public class FibonacciBenchmark
    {
        [Params(1, 17, 64, 1024)]
        public int N;

        [Benchmark]
        public BigInteger Fibonacci() => MathR.Fibonacci(N);

    }
}
