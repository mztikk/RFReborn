
BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


    Method |    N |     Mean |     Error |    StdDev |
---------- |----- |---------:|----------:|----------:|
 **Fibonacci** |    **1** | **3.681 μs** | **0.0008 μs** | **0.0007 μs** |
 **Fibonacci** |   **17** | **3.696 μs** | **0.0003 μs** | **0.0003 μs** |
 **Fibonacci** |   **64** | **3.748 μs** | **0.0017 μs** | **0.0016 μs** |
 **Fibonacci** | **1024** | **5.034 μs** | **0.0025 μs** | **0.0022 μs** |
