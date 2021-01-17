using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace RFReborn.Benchmark.Benchmarks
{
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [MarkdownExporterAttribute.Default]
    [MarkdownExporterAttribute.GitHub]
    public class FactorialBenchmark
    {
        [Params(1, 17, 64)]
        public int N;

        [Benchmark]
        public BigInteger Factorial() => MathR.Factorial(N);

    }
}
