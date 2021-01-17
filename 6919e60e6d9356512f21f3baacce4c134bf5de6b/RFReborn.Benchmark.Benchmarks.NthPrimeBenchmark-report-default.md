
BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


   Method |    N |           Mean |       Error |      StdDev |
--------- |----- |---------------:|------------:|------------:|
 **NthPrime** |    **1** |       **7.719 ns** |   **0.0007 ns** |   **0.0006 ns** |
 **NthPrime** |   **17** |     **452.310 ns** |   **0.3302 ns** |   **0.2757 ns** |
 **NthPrime** |   **64** |   **4,166.465 ns** |   **4.1986 ns** |   **3.7219 ns** |
 **NthPrime** | **1024** | **365,878.340 ns** | **454.2850 ns** | **379.3485 ns** |
