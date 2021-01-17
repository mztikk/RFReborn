
BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


              Method | SearchRegionSize | SignatureLength |      Mean |    Error |   StdDev |
-------------------- |----------------- |---------------- |----------:|---------:|---------:|
  **FindSignatureStart** |               **64** |               **8** |  **19.24 ns** | **0.004 ns** | **0.003 ns** |
 FindSignatureMiddle |               64 |               8 |  22.15 ns | 0.007 ns | 0.006 ns |
    FindSignatureEnd |               64 |               8 |  22.67 ns | 0.005 ns | 0.004 ns |
  **FindSignatureStart** |               **64** |              **16** |  **25.81 ns** | **0.006 ns** | **0.006 ns** |
 FindSignatureMiddle |               64 |              16 |  28.79 ns | 0.005 ns | 0.004 ns |
    FindSignatureEnd |               64 |              16 |  28.77 ns | 0.009 ns | 0.008 ns |
  **FindSignatureStart** |               **64** |              **32** |  **39.14 ns** | **0.083 ns** | **0.077 ns** |
 FindSignatureMiddle |               64 |              32 |  41.97 ns | 0.018 ns | 0.016 ns |
    FindSignatureEnd |               64 |              32 |  42.25 ns | 0.339 ns | 0.317 ns |
  **FindSignatureStart** |             **1024** |               **8** |  **19.24 ns** | **0.010 ns** | **0.008 ns** |
 FindSignatureMiddle |             1024 |               8 |  47.53 ns | 0.006 ns | 0.005 ns |
    FindSignatureEnd |             1024 |               8 |  75.75 ns | 0.010 ns | 0.009 ns |
  **FindSignatureStart** |             **1024** |              **16** |  **25.82 ns** | **0.005 ns** | **0.004 ns** |
 FindSignatureMiddle |             1024 |              16 |  54.01 ns | 0.011 ns | 0.011 ns |
    FindSignatureEnd |             1024 |              16 |  63.15 ns | 0.003 ns | 0.003 ns |
  **FindSignatureStart** |             **1024** |              **32** |  **39.08 ns** | **0.030 ns** | **0.025 ns** |
 FindSignatureMiddle |             1024 |              32 |  48.26 ns | 0.195 ns | 0.183 ns |
    FindSignatureEnd |             1024 |              32 |  74.19 ns | 0.192 ns | 0.180 ns |
  **FindSignatureStart** |             **8192** |               **8** |  **19.52 ns** | **0.002 ns** | **0.001 ns** |
 FindSignatureMiddle |             8192 |               8 | 211.95 ns | 0.073 ns | 0.064 ns |
    FindSignatureEnd |             8192 |               8 | 416.45 ns | 0.293 ns | 0.245 ns |
  **FindSignatureStart** |             **8192** |              **16** |  **26.39 ns** | **0.002 ns** | **0.002 ns** |
 FindSignatureMiddle |             8192 |              16 | 191.58 ns | 0.030 ns | 0.025 ns |
    FindSignatureEnd |             8192 |              16 | 468.12 ns | 0.151 ns | 0.141 ns |
  **FindSignatureStart** |             **8192** |              **32** |  **39.64 ns** | **0.072 ns** | **0.067 ns** |
 FindSignatureMiddle |             8192 |              32 | 230.86 ns | 0.133 ns | 0.125 ns |
    FindSignatureEnd |             8192 |              32 | 450.99 ns | 0.622 ns | 0.582 ns |
