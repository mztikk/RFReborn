``` ini

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|    Method |    N |     Mean |     Error |    StdDev |
|---------- |----- |---------:|----------:|----------:|
| **Fibonacci** |    **1** | **3.691 μs** | **0.0006 μs** | **0.0005 μs** |
| **Fibonacci** |   **17** | **3.700 μs** | **0.0008 μs** | **0.0006 μs** |
| **Fibonacci** |   **64** | **3.735 μs** | **0.0014 μs** | **0.0012 μs** |
| **Fibonacci** | **1024** | **5.039 μs** | **0.0018 μs** | **0.0015 μs** |
