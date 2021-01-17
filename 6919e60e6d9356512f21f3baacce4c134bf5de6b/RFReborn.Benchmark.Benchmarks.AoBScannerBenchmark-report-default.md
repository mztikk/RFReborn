
BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


              Method | SearchRegionSize | SignatureLength |      Mean |    Error |   StdDev |
-------------------- |----------------- |---------------- |----------:|---------:|---------:|
  **FindSignatureStart** |               **64** |               **8** |  **19.22 ns** | **0.002 ns** | **0.002 ns** |
 FindSignatureMiddle |               64 |               8 |  21.62 ns | 0.005 ns | 0.004 ns |
    FindSignatureEnd |               64 |               8 |  22.17 ns | 0.002 ns | 0.002 ns |
  **FindSignatureStart** |               **64** |              **16** |  **25.82 ns** | **0.010 ns** | **0.009 ns** |
 FindSignatureMiddle |               64 |              16 |  28.23 ns | 0.002 ns | 0.002 ns |
    FindSignatureEnd |               64 |              16 |  28.79 ns | 0.016 ns | 0.015 ns |
  **FindSignatureStart** |               **64** |              **32** |  **39.05 ns** | **0.002 ns** | **0.001 ns** |
 FindSignatureMiddle |               64 |              32 |  41.45 ns | 0.021 ns | 0.017 ns |
    FindSignatureEnd |               64 |              32 |  41.98 ns | 0.005 ns | 0.004 ns |
  **FindSignatureStart** |             **1024** |               **8** |  **19.23 ns** | **0.004 ns** | **0.004 ns** |
 FindSignatureMiddle |             1024 |               8 |  49.90 ns | 0.020 ns | 0.017 ns |
    FindSignatureEnd |             1024 |               8 | 113.91 ns | 0.062 ns | 0.055 ns |
  **FindSignatureStart** |             **1024** |              **16** |  **25.82 ns** | **0.006 ns** | **0.005 ns** |
 FindSignatureMiddle |             1024 |              16 |  53.72 ns | 0.011 ns | 0.010 ns |
    FindSignatureEnd |             1024 |              16 | 106.02 ns | 0.008 ns | 0.007 ns |
  **FindSignatureStart** |             **1024** |              **32** |  **39.07 ns** | **0.018 ns** | **0.016 ns** |
 FindSignatureMiddle |             1024 |              32 |  79.76 ns | 0.029 ns | 0.027 ns |
    FindSignatureEnd |             1024 |              32 | 123.41 ns | 0.034 ns | 0.030 ns |
  **FindSignatureStart** |             **8192** |               **8** |  **27.43 ns** | **0.003 ns** | **0.002 ns** |
 FindSignatureMiddle |             8192 |               8 | 229.62 ns | 0.221 ns | 0.207 ns |
    FindSignatureEnd |             8192 |               8 | 418.15 ns | 0.166 ns | 0.147 ns |
  **FindSignatureStart** |             **8192** |              **16** |  **26.37 ns** | **0.006 ns** | **0.006 ns** |
 FindSignatureMiddle |             8192 |              16 | 200.05 ns | 0.054 ns | 0.048 ns |
    FindSignatureEnd |             8192 |              16 | 474.02 ns | 0.214 ns | 0.200 ns |
  **FindSignatureStart** |             **8192** |              **32** |  **39.61 ns** | **0.012 ns** | **0.011 ns** |
 FindSignatureMiddle |             8192 |              32 | 187.64 ns | 0.250 ns | 0.234 ns |
    FindSignatureEnd |             8192 |              32 | 400.52 ns | 0.407 ns | 0.380 ns |
