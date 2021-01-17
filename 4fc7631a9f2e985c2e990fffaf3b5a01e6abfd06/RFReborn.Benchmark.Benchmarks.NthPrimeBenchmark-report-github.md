``` ini

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|   Method |    N |           Mean |       Error |      StdDev |
|--------- |----- |---------------:|------------:|------------:|
| **NthPrime** |    **1** |       **7.724 ns** |   **0.0052 ns** |   **0.0043 ns** |
| **NthPrime** |   **17** |     **445.897 ns** |   **0.0348 ns** |   **0.0272 ns** |
| **NthPrime** |   **64** |   **4,162.598 ns** |   **1.3076 ns** |   **1.0919 ns** |
| **NthPrime** | **1024** | **354,549.151 ns** | **212.5078 ns** | **198.7800 ns** |
