using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using RFReborn.Benchmark.Benchmarks;

namespace RFReborn.Benchmark
{
    internal class Program
    {
        private static readonly List<Type> s_typesToBenchmark = new List<Type>() {
            typeof(NthPrimeBenchmark),
            typeof(FactorialBenchmark),
            typeof(FibonacciBenchmark),
            typeof(AoBScannerBenchmark),
        };

        private static void Main(string[] args)
        {
            //BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<AoBScannerBenchmark>();
            foreach (Type item in s_typesToBenchmark)
            {
                BenchmarkRunner.Run(item);
            }
        }
    }
}
