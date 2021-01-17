
BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


    Method |  N |        Mean |    Error |   StdDev |
---------- |--- |------------:|---------:|---------:|
 **Factorial** |  **1** |    **15.39 ns** | **0.003 ns** | **0.004 ns** |
 **Factorial** | **17** |   **441.73 ns** | **0.273 ns** | **0.213 ns** |
 **Factorial** | **64** | **2,123.18 ns** | **1.176 ns** | **1.042 ns** |
