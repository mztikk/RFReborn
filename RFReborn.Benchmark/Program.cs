using System;
using BenchmarkDotNet.Running;

namespace RFReborn.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<AoBScannerBenchmark>();
        }
    }
}
