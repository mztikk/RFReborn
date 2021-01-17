``` ini

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|    Method |  N |        Mean |    Error |   StdDev |
|---------- |--- |------------:|---------:|---------:|
| **Factorial** |  **1** |    **20.84 ns** | **0.277 ns** | **0.259 ns** |
| **Factorial** | **17** |   **438.27 ns** | **0.244 ns** | **0.204 ns** |
| **Factorial** | **64** | **2,108.15 ns** | **2.570 ns** | **2.007 ns** |
