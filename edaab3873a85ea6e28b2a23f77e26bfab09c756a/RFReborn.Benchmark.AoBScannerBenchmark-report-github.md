``` ini

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|              Method | SearchRegionSize | SignatureLength |      Mean |    Error |   StdDev |    Median |
|-------------------- |----------------- |---------------- |----------:|---------:|---------:|----------:|
|  **FindSignatureStart** |               **64** |               **8** |  **19.26 ns** | **0.009 ns** | **0.009 ns** |  **19.25 ns** |
| FindSignatureMiddle |               64 |               8 |  30.61 ns | 0.010 ns | 0.008 ns |  30.61 ns |
|    FindSignatureEnd |               64 |               8 |  22.70 ns | 0.005 ns | 0.005 ns |  22.70 ns |
|  **FindSignatureStart** |               **64** |              **16** |  **25.83 ns** | **0.006 ns** | **0.005 ns** |  **25.83 ns** |
| FindSignatureMiddle |               64 |              16 |  28.79 ns | 0.019 ns | 0.017 ns |  28.79 ns |
|    FindSignatureEnd |               64 |              16 |  28.77 ns | 0.002 ns | 0.001 ns |  28.77 ns |
|  **FindSignatureStart** |               **64** |              **32** |  **39.05 ns** | **0.011 ns** | **0.008 ns** |  **39.05 ns** |
| FindSignatureMiddle |               64 |              32 |  42.07 ns | 0.101 ns | 0.094 ns |  42.05 ns |
|    FindSignatureEnd |               64 |              32 |  42.11 ns | 0.128 ns | 0.119 ns |  42.21 ns |
|  **FindSignatureStart** |             **1024** |               **8** |  **19.25 ns** | **0.006 ns** | **0.005 ns** |  **19.25 ns** |
| FindSignatureMiddle |             1024 |               8 |  39.87 ns | 0.004 ns | 0.003 ns |  39.87 ns |
|    FindSignatureEnd |             1024 |               8 |  99.31 ns | 0.037 ns | 0.031 ns |  99.29 ns |
|  **FindSignatureStart** |             **1024** |              **16** |  **25.85 ns** | **0.016 ns** | **0.013 ns** |  **25.84 ns** |
| FindSignatureMiddle |             1024 |              16 |  78.94 ns | 0.013 ns | 0.010 ns |  78.94 ns |
|    FindSignatureEnd |             1024 |              16 |  73.98 ns | 0.011 ns | 0.008 ns |  73.98 ns |
|  **FindSignatureStart** |             **1024** |              **32** |  **39.12 ns** | **0.077 ns** | **0.072 ns** |  **39.10 ns** |
| FindSignatureMiddle |             1024 |              32 |  59.52 ns | 0.051 ns | 0.045 ns |  59.51 ns |
|    FindSignatureEnd |             1024 |              32 | 105.84 ns | 0.034 ns | 0.026 ns | 105.84 ns |
|  **FindSignatureStart** |             **8192** |               **8** |  **19.50 ns** | **0.001 ns** | **0.001 ns** |  **19.50 ns** |
| FindSignatureMiddle |             8192 |               8 | 240.46 ns | 0.135 ns | 0.120 ns | 240.41 ns |
|    FindSignatureEnd |             8192 |               8 | 448.52 ns | 0.625 ns | 0.488 ns | 448.71 ns |
|  **FindSignatureStart** |             **8192** |              **16** |  **26.37 ns** | **0.006 ns** | **0.006 ns** |  **26.37 ns** |
| FindSignatureMiddle |             8192 |              16 | 228.12 ns | 0.014 ns | 0.011 ns | 228.12 ns |
|    FindSignatureEnd |             8192 |              16 | 447.01 ns | 0.264 ns | 0.234 ns | 446.97 ns |
|  **FindSignatureStart** |             **8192** |              **32** |  **39.61 ns** | **0.047 ns** | **0.041 ns** |  **39.60 ns** |
| FindSignatureMiddle |             8192 |              32 | 252.21 ns | 0.285 ns | 0.223 ns | 252.26 ns |
|    FindSignatureEnd |             8192 |              32 | 504.48 ns | 0.122 ns | 0.108 ns | 504.44 ns |
