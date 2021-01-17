``` ini

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|              Method | SearchRegionSize | SignatureLength |      Mean |    Error |   StdDev |
|-------------------- |----------------- |---------------- |----------:|---------:|---------:|
|  **FindSignatureStart** |               **64** |               **8** |  **19.22 ns** | **0.001 ns** | **0.001 ns** |
| FindSignatureMiddle |               64 |               8 |  22.15 ns | 0.005 ns | 0.004 ns |
|    FindSignatureEnd |               64 |               8 |  25.82 ns | 0.004 ns | 0.004 ns |
|  **FindSignatureStart** |               **64** |              **16** |  **25.84 ns** | **0.007 ns** | **0.007 ns** |
| FindSignatureMiddle |               64 |              16 |  28.77 ns | 0.004 ns | 0.003 ns |
|    FindSignatureEnd |               64 |              16 |  30.62 ns | 0.003 ns | 0.003 ns |
|  **FindSignatureStart** |             **1024** |               **8** |  **19.22 ns** | **0.005 ns** | **0.004 ns** |
| FindSignatureMiddle |             1024 |               8 |  57.61 ns | 0.013 ns | 0.011 ns |
|    FindSignatureEnd |             1024 |               8 | 122.04 ns | 0.014 ns | 0.012 ns |
|  **FindSignatureStart** |             **1024** |              **16** |  **25.82 ns** | **0.005 ns** | **0.004 ns** |
| FindSignatureMiddle |             1024 |              16 |  64.95 ns | 0.016 ns | 0.013 ns |
|    FindSignatureEnd |             1024 |              16 |  90.62 ns | 0.021 ns | 0.016 ns |
