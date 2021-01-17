using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;

namespace RFReborn.Benchmark
{
    internal class Program
    {
        private static readonly List<Type> s_typesToBenchmark = new List<Type>() {
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
